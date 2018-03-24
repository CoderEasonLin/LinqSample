﻿using ExpectedObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LinqTests;

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

        [TestMethod]
        public void find_employee_older_than_30()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.Find((e, index) => e.Age > 30 && index > 2);

            var expected = new List<Employee>()
            {
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6}
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }
    }
}

internal static class WithoutLinq
{
    public static IEnumerable<T> Find<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
                yield return item;
        }
    }

    public static IEnumerable<T> Find<T>(this IEnumerable<T> source, Func<T, int, bool> predicate)
    {
        var index = 0;
        foreach (var item in source)
        {
            if (predicate(item, index))
                yield return item;
            index++;
        }
    }
}

internal class YourOwnLinq
{
}