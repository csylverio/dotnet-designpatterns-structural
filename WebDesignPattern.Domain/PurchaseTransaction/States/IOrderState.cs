using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;
using WebDesignPattern.Domain.PurchaseTransaction.Shippings;

namespace WebDesignPattern.Domain.PurchaseTransaction.States;

public interface IOrderState
{
    OrderStatus Status { get; } // Encapsula o status atual
    void Finalize(Order order);
    PaymentResult Pay(Order order, int paymentMethodId, IPaymentGatewayFactory gatewayFactory, IPaymentRepository paymentRepository);
    void Ship(Order order, ShippingServiceContext shippingServiceContext);
    void Deliver(Order order);
    void Cancel(Order order);
}
