using System;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public interface IOrderRepository
{
    void Add(Order order);
    Order GetById(int orderId);
    void Update(Order order);
}
