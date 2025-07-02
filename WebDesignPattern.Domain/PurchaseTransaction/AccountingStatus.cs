namespace WebDesignPattern.Domain.PurchaseTransaction;

/// <summary>
/// Status contábil do pedido.
/// Representa o estado do registro contábil do pedido, como pendente, registrado ou com erro.
/// </summary>
public enum AccountingStatus
{
    Pending,
    Registered,
    Error
}
