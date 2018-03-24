using ExpectedObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var actual = products.EasonWhere(product => product.IsTopSale());

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
            var actual = employees.EasonWhere((e, index) => e.Age > 30 && index > 2);

            var expected = new List<Employee>()
            {
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6}
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void replace_http_to_https()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = urls.EasonSelect(url => url.Replace("http:", "https:"));

            var expected = new List<string>()
            {
                "https://tw.yahoo.com",
                "https://facebook.com",
                "https://twitter.com",
                "https://github.com"
        };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void retrun_length()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = urls.EasonSelect(url => url.Length);

            var expected = new List<int>()
            {
                19,
                20,
                19,
                17
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void return_age_smaller_than_25()
        {
            var employees = RepositoryFactory.GetEmployees();


            var actual = employees
                            .EasonWhere(e => e.Age < 25)
                            .EasonSelect(e => $"{e.Role}:{e.Name}");

            var actualNew = from e in employees
                            where e.Age < 25
                            select $"{e.Role}:{e.Name}";

            var expected = new List<string>()
            {
                "OP:Andy",
                "Engineer:Frank",
            };

            expected.ToExpectedObject().ShouldEqual(actualNew.ToList());
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

    public static IEnumerable<string> Select(IEnumerable<string> urls, Func<string, string> act)
    {
        foreach (var url in urls)
        {
            yield return act(url);
        }
    }

    public static IEnumerable<int> Select2(IEnumerable<string> urls, Func<string, int> act)
    {
        foreach (var url in urls)
        {
            yield return act(url);
        }
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        foreach (var item in source)
        {
            yield return selector(item);
        }
    }
}

internal static class YourOwnLinq
{
    public static IEnumerable<T> EasonWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        Console.WriteLine("Where Start");
        foreach (var item in source)
        {
            Console.WriteLine("Where Move Next");
            if (predicate(item))
            {
                Console.WriteLine("Where Yield Return");
                yield return item;
            }
        }
        Console.WriteLine("Where End");
    }

    public static IEnumerable<T> EasonWhere<T>(this IEnumerable<T> source, Func<T, int, bool> predicate)
    {
        var index = 0;
        foreach (var item in source)
        {
            if (predicate(item, index))
                yield return item;
            index++;
        }
    }

    public static IEnumerable<TResult> EasonSelect<TResult, TSource>(this IEnumerable<TSource> source,
        Func<TSource, TResult> selector)
    {
        Console.WriteLine("Select Start");
        foreach (var item in source)
        {
            Console.WriteLine("Select Yield Return");
            yield return selector(item);
        }
        Console.WriteLine("Select End");
    }
}