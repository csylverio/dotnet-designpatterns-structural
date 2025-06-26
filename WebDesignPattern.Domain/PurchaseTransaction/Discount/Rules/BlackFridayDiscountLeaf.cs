using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Discount.Rules;

public class BlackFridayDiscountLeaf : IDiscountRuleComponent
{
    public decimal CalculateDiscount(Order order)
    {
        return order.TotalAmount * 0.15m; // 15% de desconto
    }
}
