using System;

namespace WebDesignPattern.Domain.CustomerRelationshipManagement;

public class Customer
{
    public int Id { get; set; }                  // ID único (chave primária)
    public string Name { get; set; }
    public string Cpf { get; private set; }      // Imutável após definição
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; } = true;   // Valor padrão
    public DateTime RegisterDate { get; } = DateTime.Now; // Auto-inicializado


    //TODO: adicionar DataAnnotations como [Required] ou [EmailAddress]
}
