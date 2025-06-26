using System;
using WebDesignPattern.Domain.PurchaseTransaction.Discount;
using WebDesignPattern.Domain.PurchaseTransaction.Discount.Rules;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayFactory _paymentGatewayFactory;
    private readonly IDiscountConfiguration _discountConfiguration;
    private readonly IAccountingService _accountingService;

    public OrderService(IOrderRepository orderRepository, IPaymentRepository paymentRepository,
        IPaymentGatewayFactory paymentGatewayFactory, IDiscountConfiguration discountConfiguration,
        IAccountingService accountingService)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _paymentGatewayFactory = paymentGatewayFactory;
        _discountConfiguration = discountConfiguration;
        _accountingService = accountingService;
    }

    public Order GetById(int orderId)
    {
        return _orderRepository.GetById(orderId);
    }

    public Order Create(Order order)
    {
        _orderRepository.Add(order);

        return order;
    }

    public Order FinalizeOrder(Order order, string? couponCode = null)
    {
        // --------------------------------------------------------------
        // 1. Processa Desconto na plataforma utilizando composite
        // --------------------------------------------------------------

        // Composite de descontos básicos (sempre aplicados)
        var baseDiscounts = new BaseDiscountsComposite();

        // Composite de descontos promocionais (condicionais)
        var promotionalDiscounts = new PromotionalDiscountsComposite(order);

        // Adiciona descontos configuráveis
        if (_discountConfiguration.ApplyPercentageDiscount)
        {
            baseDiscounts.AddRule(new PercentageDiscountLeaf(_discountConfiguration.Percentage));
        }

        if (_discountConfiguration.ApplyFixedDiscount)
        {
            baseDiscounts.AddRule(new FixedAmountDiscountLeaf(_discountConfiguration.FixedAmount));
        }

        // Adiciona cupom se existir
        if (!string.IsNullOrEmpty(couponCode))
        {
            promotionalDiscounts.AddTemporaryPromotion(new CouponDiscountLeaf(couponCode));
        }

        // Calcula descontos básicos
        decimal baseDiscount = baseDiscounts.CalculateDiscount(order);

        // Calcula descontos promocionais
        decimal promotionalDiscount = promotionalDiscounts.CalculateDiscount(order);

        // Aplica os descontos (poderia ter lógica de limite máximo aqui)
        decimal totalDiscount = baseDiscount + promotionalDiscount;

        // Garante que o desconto não ultrapasse o valor máximo permitido
        decimal maxAllowedDiscount = order.TotalAmount * 0.3m; // Máximo de 30%
        totalDiscount = Math.Min(totalDiscount, maxAllowedDiscount);

        // Aplica o desconto e finaliza o pedido
        order.Discount = totalDiscount;
        order.Status = OrderStatus.AwaitingPayment;

        // Registra detalhes dos descontos aplicados
        order.DiscountDetail = new DiscountDetail
        {
            BaseDiscount = baseDiscount,
            PromotionalDiscount = promotionalDiscount,
            FinalDiscount = totalDiscount
        };


        // --------------------------------------------------------------
        // 2. Registra na contabilidade usando o Facade
        // --------------------------------------------------------------
        var accountingResult = _accountingService.RegisterSale(order);

        if (accountingResult.Success)
        {
            order.AccountingStatus = AccountingStatus.Registered;
            order.AccountingDocument = accountingResult.DocumentNumber;
            order.AccountingDate = accountingResult.AccountingDate;
        }
        else
        {
            // Pode-se optar por lançar exceção ou apenas registrar o erro
            order.AccountingStatus = AccountingStatus.Error;
            order.AccountingMessage = accountingResult.Message;            
        }

        // --------------------------------------------------------------
        // 3. Atualiza o pedido com detalhes de desconto e status contábil 
        // --------------------------------------------------------------
        _orderRepository.Update(order);

        return order;
    }

    public PaymentResult MakePayment(Order order, int paymentMethodId)
    {
        try
        {
            // Cria a entidade Payment antes de processar
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                PaymentMethod = paymentMethodId,
                Status = PaymentStatus.Pending
            };

            // Factory Pattern - para selecionar o gateway (adapter) correto
            var paymentGateway = _paymentGatewayFactory.Create(paymentMethodId);

            // Adapter Pattern - Interface IPaymentGateway é implementada por adaptadores
            var paymentGatewayResponse = paymentGateway.ProcessPayment(order.TotalAmount);

            /* 
            ---------------------------------------------
            Utilizando adapter diretamente
            ---------------------------------------------
            IPaymentGateway paymentGateway = null;
            if (paymentMethodId == 1)
            {
                paymentGateway = new PagSeguroAdapter(_provider.GetRequiredService<PagSeguroService>());
            }
            else if (paymentMethodId == 2)
            {
                paymentGateway = new PayPalAdapter(_provider.GetRequiredService<PayPalApi>());
            }
            */


            // Atualiza a entidade Payment com a resposta
            payment.TransactionId = paymentGatewayResponse.GatewayTransactionId;
            payment.GatewayResponse = paymentGatewayResponse.RawResponse;
            payment.Status = paymentGatewayResponse.IsSuccess ? PaymentStatus.Approved : PaymentStatus.Declined;
            payment.ErrorMessage = paymentGatewayResponse.ErrorMessage;

            if (paymentGatewayResponse.IsSuccess)
            {
                // Observer Pattern - Poderia notificar outros sistemas aqui (APRESENTAÇÃO 3)
                // order.MarkAsPaid(payment.TransactionId);
                order.Status = OrderStatus.PaymentApproved; // Muda para "em preparação"

                // Salva tanto o pedido quanto o pagamento
                _paymentRepository.Add(payment);
                _orderRepository.Update(order);

                // Retorna resultado positivo
                return new PaymentResult
                {
                    Success = true,
                    PaymentId = payment.Id,
                    TransactionId = payment.TransactionId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = paymentMethodId,
                    Status = "Aprovado",
                    NextStep = order.Status // Retorna o novo status do pedido
                };
            }
            else
            {
                // Salva o pagamento mesmo falhado para histórico
                _paymentRepository.Add(payment);

                return new PaymentResult
                {
                    Success = false,
                    PaymentId = payment.Id,
                    Status = "Recusado",
                    ErrorMessage = paymentGatewayResponse.ErrorMessage,
                    NextStep = OrderStatus.PaymentFailed
                };
            }
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                Success = false,
                Status = "Erro",
                ErrorMessage = ex.Message,
                NextStep = OrderStatus.PaymentError
            };
        }
    }
}
