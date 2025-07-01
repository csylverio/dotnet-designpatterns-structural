using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Domain.PurchaseTransaction.States;

public class DeliveredState : IOrderState
{
    public OrderStatus Status => OrderStatus.Delivered;

    public void Finalize(Order order) 
        => throw new InvalidOperationException("Pedido já finalizado e entregue.");

    public PaymentResult Pay(Order order, int paymentMethodId, IPaymentGatewayFactory gatewayFactory, IPaymentRepository paymentRepository)
        => throw new InvalidOperationException("Pedido já pago e entregue.");

    public void Ship(Order order) 
        => throw new InvalidOperationException("Pedido já entregue não pode ser reenviado.");

    public void Deliver(Order order)
    {
        // Opcional: Pode lançar exceção ou simplesmente ignorar, pois já está entregue
        Console.WriteLine("Pedido já consta como entregue.");
    }

    public void Cancel(Order order)
    {
        // Se necessário, poderia implementar lógica de devolução pós-entrega
        throw new InvalidOperationException(
            "Pedido já entregue. Inicie um processo de devolução se necessário."
        );
    }
}
