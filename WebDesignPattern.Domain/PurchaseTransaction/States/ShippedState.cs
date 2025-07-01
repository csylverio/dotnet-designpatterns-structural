using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Domain.PurchaseTransaction.States;

public class ShippedState : IOrderState
{
    public OrderStatus Status => OrderStatus.Shipped;

    public void Finalize(Order order) 
        => throw new InvalidOperationException("Pedido já finalizado.");

    public PaymentResult Pay(Order order, int paymentMethodId, IPaymentGatewayFactory gatewayFactory, IPaymentRepository paymentRepository)
        => throw new InvalidOperationException("Pedido já pago.");

    public void Ship(Order order) 
        => throw new InvalidOperationException("Pedido já enviado.");

     public void Deliver(Order order)
    {
        Console.WriteLine("Pedido marcado como entregue.");
        order.SetState(new DeliveredState());
    }

    public void Cancel(Order order) 
        => throw new InvalidOperationException("Pedido já enviado. Não pode ser cancelado.");
}