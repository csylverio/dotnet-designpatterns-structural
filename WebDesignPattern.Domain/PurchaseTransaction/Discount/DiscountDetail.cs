using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Discount;

public class DiscountDetail
{
    public decimal BaseDiscount { get; set; }
    public decimal PromotionalDiscount { get; set; }
    public decimal FinalDiscount { get; set; }
}
