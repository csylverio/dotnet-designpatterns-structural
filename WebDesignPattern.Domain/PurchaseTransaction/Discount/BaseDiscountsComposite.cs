using System;
using WebDesignPattern.Domain.PurchaseTransaction.Discount.Rules;

namespace WebDesignPattern.Domain.PurchaseTransaction.Discount;

public class BaseDiscountsComposite : IDiscountRuleComponent
{
    private readonly List<IDiscountRuleComponent> _baseRules = new();

    public BaseDiscountsComposite()
    {
        // Regras básicas que sempre se aplicam
        _baseRules.Add(new FirstPurchaseDiscountLeaf());
        _baseRules.Add(new VolumeDiscountLeaf());
    }

    public void AddRule(IDiscountRuleComponent rule)
    {
        _baseRules.Add(rule);
    }

    public decimal CalculateDiscount(Order order)
    {
        return _baseRules.Sum(rule => rule.CalculateDiscount(order));
    }
}
