using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Domain.PurchaseTransaction.States;

public class PaymentApprovedState : IOrderState
{
    public OrderStatus Status => OrderStatus.PaymentApproved;

    public void Finalize(Order order) 
        => throw new InvalidOperationException("Pedido já finalizado.");

    public PaymentResult Pay(Order order, int paymentMethodId, IPaymentGatewayFactory gatewayFactory, IPaymentRepository paymentRepository)
        => throw new InvalidOperationException("Pedido já pago.");

    public void Ship(Order order)
    {
        Console.WriteLine("Enviando pedido...");
        order.SetState(new ShippedState());
    }

    public void Deliver(Order order) 
        => throw new InvalidOperationException("Pedido não enviado. Envie antes de entregar.");

    public void Cancel(Order order)
    {
        Console.WriteLine("Pedido cancelado (após pagamento). Reembolso necessário.");
        order.SetState(new CancelledState());
    }
}
