using System;
using WebDesignPattern.Domain.InventoryManagement;

namespace WebDesignPattern.Infra.Data;

public class ProductRepository : IProductRepository
{
    public Product GetById(int productId)
    {
        Console.WriteLine("ProductRepository.GetById");
        return new Product()
        {
            Id = productId
        };
    }
}
