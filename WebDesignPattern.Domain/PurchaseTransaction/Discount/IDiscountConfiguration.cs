using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Discount;

public interface IDiscountConfiguration
{
    bool ApplyPercentageDiscount { get; }
    decimal Percentage { get; }
    bool ApplyFixedDiscount { get; }
    decimal FixedAmount { get; }
}
