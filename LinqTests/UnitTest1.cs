using ExpectedObjects;
using LinqTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace LinqTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = WithoutLinq.FindProductByPrice(products, 200, 500);

            var expected = new List<Product>()
            {
                new Product()
                {
                    Id = 2,
                    Price = 210,
                    Cost = 21,
                    Supplier = "Yahoo"
                },
                new Product()
                {
                    Id = 3,
                    Price = 310,
                    Cost = 31,
                    Supplier = "Odd-e"
                },
                new Product()
                {
                    Id = 4,
                    Price = 410,
                    Cost = 41,
                    Supplier = "Odd-e"
                }
            };

            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void linq_find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.Where(x => x.Price >= 200 && x.Price <= 500);

            var expected = new List<Product>()
            {
                new Product()
                {
                    Id = 2,
                    Price = 210,
                    Cost = 21,
                    Supplier = "Yahoo"
                },
                new Product()
                {
                    Id = 3,
                    Price = 310,
                    Cost = 31,
                    Supplier = "Odd-e"
                },
                new Product()
                {
                    Id = 4,
                    Price = 410,
                    Cost = 41,
                    Supplier = "Odd-e"
                }
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }
    }
}

internal class WithoutLinq
{
    public static List<Product> FindProductByPrice(IEnumerable<Product> products, int lowBoundary, int highBoundary)
    {
        List<Product> results = new List<Product>();
        foreach (var product in products)
        {
            if (product.Price >= lowBoundary && product.Price <= highBoundary)
                results.Add(product);
        }
        return results;
    }
}

internal class YourOwnLinq
{
}