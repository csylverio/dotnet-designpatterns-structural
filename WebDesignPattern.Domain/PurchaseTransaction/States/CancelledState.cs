using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;
using WebDesignPattern.Domain.PurchaseTransaction.Shippings;

namespace WebDesignPattern.Domain.PurchaseTransaction.States;

public class CancelledState : IOrderState
{
    public OrderStatus Status => OrderStatus.Canceled;

    public void Finalize(Order order) 
        => throw new InvalidOperationException("Pedido cancelado não pode ser finalizado.");

    public PaymentResult Pay(Order order, int paymentMethodId, IPaymentGatewayFactory gatewayFactory, IPaymentRepository paymentRepository)
        => throw new InvalidOperationException("Pedido cancelado não pode ser pago.");

    public void Ship(Order order, ShippingServiceContext shippingServiceContext) 
        => throw new InvalidOperationException("Pedido cancelado não pode ser enviado.");

    public void Deliver(Order order) 
        => throw new InvalidOperationException("Pedido cancelado não pode ser entregue.");

    public void Cancel(Order order) 
        => Console.WriteLine("Pedido já cancelado.");
}