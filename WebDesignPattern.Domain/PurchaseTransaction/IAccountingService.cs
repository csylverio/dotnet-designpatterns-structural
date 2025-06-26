using System;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public interface IAccountingService
{
    AccountingResult RegisterSale(Order order);
}
