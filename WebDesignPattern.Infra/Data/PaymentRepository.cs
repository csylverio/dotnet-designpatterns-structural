using System;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Infra.Data;

public class PaymentRepository : IPaymentRepository
{
    public void Add(Payment payment)
    {
        Console.WriteLine("PaymentRepository.Add");
    }
}
