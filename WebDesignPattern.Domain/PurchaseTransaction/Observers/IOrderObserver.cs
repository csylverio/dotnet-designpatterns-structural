using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Observers;

public interface IOrderObserver
{
    void OnOrderUpdated(Order order, OrderEventType eventType);
}
