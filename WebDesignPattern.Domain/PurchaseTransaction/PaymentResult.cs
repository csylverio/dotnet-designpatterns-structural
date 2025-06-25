using System;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public class PaymentResult
{
    public bool Success { get; set; }
    public int PaymentId { get; set; }           // ID do registro no banco
    public string TransactionId { get; set; }     // ID do gateway
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PaymentMethod { get; set; }
    public string Status { get; set; }           // Status descritivo
    public string ErrorMessage { get; set; }
    public OrderStatus NextStep { get; set; }     // Próximo status do pedido
}
