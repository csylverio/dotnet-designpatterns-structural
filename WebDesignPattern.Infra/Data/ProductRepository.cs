using System;
using WebDesignPattern.Domain.InventoryManagement;

namespace WebDesignPattern.Infra.Data;

public class ProductRepository : IProductRepository
{
    public Product GetById(int productId)
    {
        Console.WriteLine("ProductRepository.GetById");
        return OrderFakerGenerator.ProductFaker;
    }

    public void IncreaseStock(int productId, int quantity)
    {
        Console.WriteLine("ProductRepository.IncreaseStock");
    }

    public void DecreaseStock(int productId, int quantity)
    {
        Console.WriteLine("ProductRepository.DecreaseStock");
    }

}
