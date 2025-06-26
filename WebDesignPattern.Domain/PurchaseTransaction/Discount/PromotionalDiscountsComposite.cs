using System;
using WebDesignPattern.Domain.PurchaseTransaction.Discount.Rules;

namespace WebDesignPattern.Domain.PurchaseTransaction.Discount;

public class PromotionalDiscountsComposite : IDiscountRuleComponent
{
    private readonly List<IDiscountRuleComponent> _promotionalRules = new();
    private readonly DateTime _currentDate;

    public PromotionalDiscountsComposite(Order order)
    {
        _currentDate = DateTime.Now;
        
        // Regras promocionais condicionais
        if (IsBlackFridayPeriod())
        {
            _promotionalRules.Add(new BlackFridayDiscountLeaf());
        }
        
        if (IsBirthdayMonth(order.Customer.BirthDate))
        {
            _promotionalRules.Add(new BirthdayDiscountLeaf());
        }
    }

    private bool IsBlackFridayPeriod()
    {
        return _currentDate.Month == 11 && _currentDate.Day >= 20 && _currentDate.Day <= 30;
    }

    private bool IsBirthdayMonth(DateTime customerBirthDate)
    {
        return _currentDate.Month == customerBirthDate.Month;
    }

    public void AddTemporaryPromotion(IDiscountRuleComponent promotion)
    {
        _promotionalRules.Add(promotion);
    }

    public decimal CalculateDiscount(Order order)
    {
        return _promotionalRules.Sum(rule => rule.CalculateDiscount(order));
    }
}
