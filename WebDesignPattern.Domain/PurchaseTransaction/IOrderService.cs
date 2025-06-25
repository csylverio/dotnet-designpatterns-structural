using System;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public interface IOrderService
{
    Order Create(Order order);
    Order GetById(int orderId);
    PaymentResult MakePayment(Order order, int paymentMethodId);
}
