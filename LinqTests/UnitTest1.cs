using System;
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
            var actual = products.Find(product => product.IsTopSale());

            var expected = new List<Product>()
            {
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

        [TestMethod]
        public void linq_find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.Where(x => x.Price >= 200 && x.Price <= 500 && x.Cost >= 30);

            var expected = new List<Product>()
            {
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

internal static class WithoutLinq
{
    public static IEnumerable<Product> Find(this IEnumerable<Product> products, Func<Product, bool> predicate)
    {
        foreach (var product in products)
        {
            if (predicate(product))
                yield return product;
        }
    }
}

internal class YourOwnLinq
{
}