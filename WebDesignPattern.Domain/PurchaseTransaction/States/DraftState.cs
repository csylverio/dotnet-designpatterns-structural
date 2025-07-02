using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;
using WebDesignPattern.Domain.PurchaseTransaction.Shippings;

namespace WebDesignPattern.Domain.PurchaseTransaction.States;

public class DraftState : IOrderState
{
    public OrderStatus Status => OrderStatus.Draft;

    public void Finalize(Order order)
    {
        Console.WriteLine("Finalizando pedido...");
        order.SetState(new AwaitingPaymentState());
    }

    public PaymentResult Pay(Order order, int paymentMethodId, IPaymentGatewayFactory gatewayFactory, IPaymentRepository paymentRepository)
        => throw new InvalidOperationException("Pedido não finalizado. Finalize antes de pagar.");

    public void Ship(Order order, ShippingServiceContext shippingServiceContext) 
        => throw new InvalidOperationException("Pedido não pago. Pague antes de enviar.");

    public void Deliver(Order order) 
        => throw new InvalidOperationException("Pedido não enviado. Envie antes de entregar.");

    public void Cancel(Order order)
    {
        Console.WriteLine("Pedido cancelado (rascunho).");
        order.SetState(new CancelledState());
    }
}
