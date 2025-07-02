using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Observers;

public enum OrderEventType
{
    OrderCreated,
    PaymentApproved,
    PaymentFailed,
    OrderShipped,
    OrderDelivered,
    OrderCancelled
}
