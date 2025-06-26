using System;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public interface IOrderService
{
    Order GetById(int orderId);
    Order Create(Order order);
    PaymentResult MakePayment(Order order, int paymentMethodId);
    Order FinalizeOrder(Order order, string? couponCode);
}
