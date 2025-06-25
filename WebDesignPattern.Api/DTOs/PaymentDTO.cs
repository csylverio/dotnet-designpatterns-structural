using System;

namespace WebDesignPattern.Api.DTOs;

public class PaymentDTO
{
    public int OrderId { get; set; }
    public int PaymentMethodId { get; set; }
    public string? CardNumber { get; set; }      // Para cartões (opcional)
    public string? LastFourDigits { get; set; }   // Últimos 4 dígitos do cartão
    public string? CustomerId { get; set; }      // Identificador do comprador (opcional)
}
