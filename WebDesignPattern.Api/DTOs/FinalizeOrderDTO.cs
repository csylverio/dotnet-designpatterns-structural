using System;
using WebDesignPattern.Domain.PurchaseTransaction;

namespace WebDesignPattern.Api.DTOs;

public class FinalizeOrderDTO
{
    public int OrderId { get; set; }
    public string? CouponCode { get; set; }
}
