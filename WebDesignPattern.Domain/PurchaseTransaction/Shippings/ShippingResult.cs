using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Shippings;

public class ShippingResult
{
    public string TrackingNumber { get; set; }
    /// <summary>
    /// Operadora de transporte responsável pelo envio
    /// </summary>
    public string Carrier { get; set; }
    public DateTime EstimatedDelivery { get; set; }
    public decimal ShippingCost { get; set; }
    public string Notes { get; set; }
}
