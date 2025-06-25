using System;
using WebDesignPattern.Domain.PurchaseTransaction;

namespace WebDesignPattern.Infra.Data;

public class OrderRepository : IOrderRepository
{
    public void Add(Order order)
    {
       Console.WriteLine("OrderRepository.Add");
    }

    public void Update(Order order)
    {
        throw new NotImplementedException();
    }
}
