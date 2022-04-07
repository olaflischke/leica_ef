using Microsoft.VisualStudio.TestTools.UnitTesting;
using nm = NorthwindDalCore.Model;
using fa = NorthwindDalCore.ModelFluentApi;
using System;
using System.Linq;

namespace NorthwindUnitTest
{
    [TestClass]
    public class DalUnitTest
    {
        [TestMethod]
        public void IsContextReading()
        {
            nm.NorthwindContext context = new nm.NorthwindContext();
            nm.Customer? ho = context.Customers.Find("ABCDE");

            Console.WriteLine($"{ho.City} {ho.Country}");
            //Assert.AreEqual(92, context.Customers.Count());
        }

        [TestMethod]
        public void IsContextWithFluentApiReading()
        {
            fa.NorthwindContext context = new();
            //Assert.AreEqual(92, context.Customers.Count());
            fa.Customer? ho = context.Customers.Find("ABCDE");

            Console.WriteLine($"{ho.City} {ho.Country}");
        }

    }
}