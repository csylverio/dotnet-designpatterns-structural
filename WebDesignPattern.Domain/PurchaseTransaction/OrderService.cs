using System;
using WebDesignPattern.Domain.PurchaseTransaction.Discount;
using WebDesignPattern.Domain.PurchaseTransaction.Discount.Rules;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;
using WebDesignPattern.Domain.PurchaseTransaction.Observers;
using WebDesignPattern.Domain.PurchaseTransaction.Shippings;
using WebDesignPattern.Domain.PurchaseTransaction.Validators;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayFactory _paymentGatewayFactory;
    private readonly IDiscountConfiguration _discountConfiguration;
    private readonly IAccountingService _accountingService;
    private readonly ShippingServiceContext _shippingServiceContext;
    private readonly OrderEventManager _eventManager;
    private readonly IOrderValidator _validator;

    public OrderService(IOrderRepository orderRepository, IPaymentRepository paymentRepository,
        IPaymentGatewayFactory paymentGatewayFactory, IDiscountConfiguration discountConfiguration,
        IAccountingService accountingService, ShippingServiceContext shippingServiceContext,
        OrderEventManager eventManager, IOrderValidator validator)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _paymentGatewayFactory = paymentGatewayFactory;
        _discountConfiguration = discountConfiguration;
        _accountingService = accountingService;
        _shippingServiceContext = shippingServiceContext;
        _eventManager = eventManager;
        _validator = validator;
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
        order.FinalizeOrder(); // Delega para o estado atual

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
        _eventManager.Notify(order, OrderEventType.OrderCreated);

        return order;
    }

    public PaymentResult MakePayment(Order order, int paymentMethodId)
    {
        try
        {
            PaymentResult paymentResult = order.MakePayment(paymentMethodId, _paymentGatewayFactory, _paymentRepository);

            _orderRepository.Update(order);

            if (paymentResult.Success)
                _eventManager.Notify(order, OrderEventType.PaymentApproved);
            else
                _eventManager.Notify(order, OrderEventType.PaymentFailed);


            return paymentResult;
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

    public Order Ship(Order order, int ShippingMethodId)
    {
        order.ShippingMethodId = ShippingMethodId;
        // --------------------------------------------------------------
        //  Exemplo de Validação de Pedido usando Chain of Responsibility
        // --------------------------------------------------------------
        var validationResult = _validator.Validate(order);
        if (!validationResult.IsValid)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        order.Ship(_shippingServiceContext); // Delega para o estado atual

        _orderRepository.Update(order);

        _eventManager.Notify(order, OrderEventType.OrderShipped);

        return order;
    }
}
