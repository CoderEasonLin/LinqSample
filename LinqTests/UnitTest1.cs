using ExpectedObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
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

        [TestMethod]
        public void take()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = WithoutLinq.EasonTake(employees, 2);

            var expected = new List<Employee>()
            {
                new Employee{Name="Joe", Role=RoleType.Engineer, MonthSalary=100, Age=44, WorkingYear=2.6 } ,
                new Employee{Name="Tom", Role=RoleType.Engineer, MonthSalary=140, Age=33, WorkingYear=2.6} ,
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void skip()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.EasonSkip(6);

            var expected = new List<Employee>()
            {
                new Employee {Name = "Frank", Role = RoleType.Engineer, MonthSalary = 120, Age = 16, WorkingYear = 2.6},
                new Employee {Name = "Joey", Role = RoleType.Engineer, MonthSalary = 250, Age = 40, WorkingYear = 2.6},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void groupSalary()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = WithoutLinq.JoeyGroup(employees, 3, e=>e.MonthSalary);

            var expected = new List<int>()
            {
                620,
                540,
                370
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void Take2()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.EasonTakeWhile(e => e.MonthSalary > 150, 2);

            var expected = new List<Employee>()
            {
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void SkipWhile()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.EasonSkipWhile(e => e.MonthSalary < 150, 3);

            var expected = new List<Employee>()
            {
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Mary", Role=RoleType.OP, MonthSalary=180, Age=26, WorkingYear=2.6} ,
                new Employee{Name="Frank", Role=RoleType.Engineer, MonthSalary=120, Age=16, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6},
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

    public static IEnumerable<TSource> EasonTake<TSource>(this IEnumerable<TSource> employees, int count)
    {
        var enumerator = employees.GetEnumerator();
        while (enumerator.MoveNext() && --count >= 0)
        {
            yield return enumerator.Current;
        }
    }

    public static IEnumerable<TSource> EasonSkip<TSource>(this IEnumerable<TSource> source, int count)
    {
        var enumerator = source.GetEnumerator();
        var index = 0;
        while (enumerator.MoveNext())
        {
            index++;
            if (index <= count)
                continue;
            yield return enumerator.Current;
        }
    }

    public static IEnumerable<TResult> EasonGroupSalary<TResult, TSource>(IEnumerable<TSource> sources, Func<TSource, int, TResult> doer)
    {
        var enumerator = sources.GetEnumerator();
        var index = 0;
        while (enumerator.MoveNext())
        {
            yield return doer(enumerator.Current, index);
            index++;
        }
    }

    public static IEnumerable<IEnumerable<TSource>> EasonGroup<TSource>(IEnumerable<TSource> sources, int count)
    {
        var enumerator = sources.GetEnumerator();
        var result = new List<TSource>();
        var index = 0;
        while (enumerator.MoveNext())
        {
            if(index % count != 0)
                result.Add(enumerator.Current);
            else
            {
                yield return result;
                result.Clear();
            }
            index++;
        }
    }

    public static IEnumerable<int> JoeyGroup<TSource>(IEnumerable<TSource> source, int pageSize, Func<TSource, int> func)
    {
        var index = 0;
        while (index < source.Count())
        {
            yield return source.EasonSkip(index).EasonTake(pageSize).Sum(func);
            index += pageSize;
        }
    }

    public static IEnumerable<TSource> EasonTakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, int count)
    {
        var enumerator = source.GetEnumerator();
        var tookCount = 0;
        while (tookCount < count && enumerator.MoveNext())
        {
            if (predicate(enumerator.Current))
            {
                yield return enumerator.Current;
                tookCount++;
            }
        }
    }

    public static IEnumerable<TSource> EasonSkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, int count)
    {
        var enumerator = source.GetEnumerator();
        var skippedCount = 0;
        while (enumerator.MoveNext())
        {
            if (skippedCount < count && predicate(enumerator.Current))
            {
                skippedCount++;
                continue;
            }
            yield return enumerator.Current;
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