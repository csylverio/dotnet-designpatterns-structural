using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayFactory _paymentGatewayFactory;

    public OrderService(IOrderRepository orderRepository, IPaymentRepository paymentRepository, IPaymentGatewayFactory paymentGatewayFactory)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _paymentGatewayFactory = paymentGatewayFactory;
    }

    public Order Create(Order order)
    {
        _orderRepository.Add(order);

        return order;
    }

    public Order GetById(int orderId)
    {
        throw new NotImplementedException();
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

            // Factory Pattern para selecionar o gateway correto
            var paymentGateway = _paymentGatewayFactory.Create(paymentMethodId);

            // Strategy Pattern - Processamento específico por método de pagamento (APRESENTAÇÃO 3)
            // Adapter - Gateway utiliza internamente padrão adapter para integrar
            var paymentGatewayResponse = paymentGateway.ProcessPayment(order.TotalAmount);

            // Atualiza a entidade Payment com a resposta
            payment.TransactionId = paymentGatewayResponse.GatewayTransactionId;
            payment.GatewayResponse = paymentGatewayResponse.RawResponse;
            payment.Status = paymentGatewayResponse.IsSuccess ? PaymentStatus.Approved : PaymentStatus.Declined;
            payment.ErrorMessage = paymentGatewayResponse.ErrorMessage;

            if (paymentGatewayResponse.IsSuccess)
            {
                // Observer Pattern - Poderia notificar outros sistemas aqui (APRESENTAÇÃO 3)
                order.MarkAsPaid(payment.TransactionId);
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
