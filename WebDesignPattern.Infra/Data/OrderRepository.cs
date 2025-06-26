using System;
using Bogus;
using WebDesignPattern.Domain.CustomerRelationshipManagement;
using WebDesignPattern.Domain.PurchaseTransaction;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Infra.Data;

public class OrderRepository : IOrderRepository
{
    public Order GetById(int orderId)
    {
        Console.WriteLine("OrderRepository.GetById");
        return OrderFakerGenerator.GenerateFakeOrder();
    }

    public void Add(Order order)
    {
        Console.WriteLine("OrderRepository.Add");
    }

    public void Update(Order order)
    {
        Console.WriteLine("OrderRepository.Update");
    }
}
