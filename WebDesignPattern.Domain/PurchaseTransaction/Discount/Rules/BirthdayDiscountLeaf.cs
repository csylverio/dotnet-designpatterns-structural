using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Discount.Rules;

public class BirthdayDiscountLeaf : IDiscountRuleComponent
{
    public decimal CalculateDiscount(Order order)
    {
        return 100; // R$100 de desconto no mês de aniversário
    }
}
