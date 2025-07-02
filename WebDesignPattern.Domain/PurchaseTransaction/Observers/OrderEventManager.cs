using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Observers;

public class OrderEventManager
{
    private readonly List<IOrderObserver> _observers = new();

    public void Subscribe(IOrderObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IOrderObserver observer) => _observers.Remove(observer);

    public void Notify(Order order, OrderEventType eventType)
    {
        foreach (var observer in _observers)
        {
            observer.OnOrderUpdated(order, eventType);
        }
    }
}
