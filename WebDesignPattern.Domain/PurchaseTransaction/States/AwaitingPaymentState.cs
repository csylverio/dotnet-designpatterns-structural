using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Domain.PurchaseTransaction.States;

public class AwaitingPaymentState : IOrderState
{
    public OrderStatus Status => OrderStatus.AwaitingPayment;

    public void Finalize(Order order)
        => throw new InvalidOperationException("Pedido já finalizado.");

    public PaymentResult Pay(Order order, int paymentMethodId, IPaymentGatewayFactory gatewayFactory, IPaymentRepository paymentRepository)
    {
        Console.WriteLine("Processando pagamento...");

        // 1. Validação básica (opcional)
        if (order.TotalAmount <= 0)
            return new PaymentResult { Success = false, ErrorMessage = "Valor do pedido inválido." };


        // Cria a entidade Payment antes de processar
        var payment = new Payment
        {
            OrderId = order.Id,
            Amount = order.TotalAmount,
            PaymentMethod = paymentMethodId,
            Status = PaymentStatus.Pending
        };

        // 2. Factory Pattern - Seleciona o gateway correto
        var paymentGateway = gatewayFactory.Create(paymentMethodId);

        // 3. Adapter Pattern - Processa o pagamento
        var paymentGatewayResponse = paymentGateway.ProcessPayment(order.TotalAmount);

        // Atualiza a entidade Payment com a resposta
        payment.TransactionId = paymentGatewayResponse.GatewayTransactionId;
        payment.GatewayResponse = paymentGatewayResponse.RawResponse;
        payment.Status = paymentGatewayResponse.IsSuccess ? PaymentStatus.Approved : PaymentStatus.Declined;
        payment.ErrorMessage = paymentGatewayResponse.ErrorMessage;


        // 4. Atualiza o estado do pedido se o pagamento for aprovado
        if (paymentGatewayResponse.IsSuccess)
        {
            Console.WriteLine("Pagamento aprovado. Atualizando estado do pedido...");
            paymentRepository.Add(payment);

            order.SetState(new PaymentApprovedState()); // Próximo estado
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
            // Pode-se optar por mudar para um estado de falha (ex: PaymentFailedState)
            Console.WriteLine("Pagamento recusado. Atualizando estado do pedido...");
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

    public void Ship(Order order)
        => throw new InvalidOperationException("Pedido não pago. Pague antes de enviar.");

    public void Deliver(Order order)
        => throw new InvalidOperationException("Pedido não enviado. Envie antes de entregar.");

    public void Cancel(Order order)
    {
        Console.WriteLine("Pedido cancelado (antes do pagamento).");
        order.SetState(new CancelledState());
    }
}
