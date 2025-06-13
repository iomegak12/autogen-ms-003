using System.Collections.Generic;
using System.Linq;

namespace ProductLibrary
{
    public class InMemoryProductService : IProductService
    {
        private readonly List<Product> _products;

        public InMemoryProductService()
        {
            _products = new List<Product>
            {
                new Product { ProductId = 1, Title = "Gaming Laptop", QuantityAvailable = 10, Description = "High performance laptop", UnitPrice = 1200.00m },
                new Product { ProductId = 2, Title = "Personal Laptop", QuantityAvailable = 50, Description = "Wireless mouse", UnitPrice = 25.99m },
                new Product { ProductId = 3, Title = "Keyboard", QuantityAvailable = 30, Description = "Mechanical keyboard", UnitPrice = 75.50m },
                new Product { ProductId = 4, Title = "Monitor", QuantityAvailable = 20, Description = "24-inch monitor", UnitPrice = 200.00m }
            };
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            return _products.Where(p => p.Title.Contains(name, System.StringComparison.OrdinalIgnoreCase));
        }

        public Product GetProductDetails(int productId)
        {
            return _products.FirstOrDefault(p => p.ProductId == productId);
        }
    }
}