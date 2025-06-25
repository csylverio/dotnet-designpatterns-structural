using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Financial;

public interface IPaymentRepository
{
    void Add(Payment payment);
}
