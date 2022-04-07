using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindDal;
using System;
using System.Linq;

namespace NorthwindUnitTests
{
    [TestClass]
    public class NorthwindEditTests
    {
        NorthwindContext context;

        public NorthwindEditTests()
        {
            context = new NorthwindContext();
            context.Database.Log = LogIt;
        }

        private void LogIt(string logString)
        {
            Console.WriteLine(logString);
        }

        [TestMethod]
        public void IsOrderForCustomerSaved()
        {
            Customer alfki = context.Customers.Find("ALFKI");

            Product chai = context.Products.Find(1);

            Order_Detail orderLine = new Order_Detail();
            orderLine.Product = chai;
            orderLine.Quantity = 15;
            orderLine.UnitPrice = chai.UnitPrice ?? 0;
            orderLine.Discount = 0;
            orderLine.FreiText = " ";

            Order order = new Order();
            order.Order_Details.Add(orderLine);
            order.Customer = alfki;

            // Neue Order dem Context bekanntmachen
            context.Orders.Add(order);

            // Nicht nötig, Context erkennt Abhängigkeiten von selbst
            //context.Order_Details.AddRange(order.Order_Details);

            context.SaveChanges();

        }

        [TestMethod]
        public void IsOrderStateChanging()
        {
            Order order;
            Order_Detail line1;

            using (NorthwindContext ctx1 = new NorthwindContext())
            {
                order = ctx1.Orders.Include("Order_Details").FirstOrDefault(od => od.ID == 11011);

                line1 = order.Order_Details.FirstOrDefault();


                Console.WriteLine($"Context 1: Order-State {ctx1.Entry(order).State}");
                Console.WriteLine($"Context 1: OrderDetail-State {ctx1.Entry(line1).State}");

            }

            using (NorthwindContext ctx2 = new NorthwindContext())
            {

                Console.WriteLine($"Context 2, before attach: Order-State {ctx2.Entry(order).State}");
                Console.WriteLine($"Context 2, before attach: OrderDetail-State {ctx2.Entry(line1).State}");
                
                ctx2.Orders.Attach(order);

                line1.Quantity = 101;

                Console.WriteLine($"Context 2, after attach: Order-State {ctx2.Entry(order).State}");
                Console.WriteLine($"Context 2, after attach: OrderDetail-State {ctx2.Entry(line1).State}");

                //ctx2.Entry(order).State = System.Data.Entity.EntityState.Modified;

                Console.WriteLine($"Context 2, after StateChange: Order-State {ctx2.Entry(order).State}");
                Console.WriteLine($"Context 2, after StateChange: OrderDetail-State {ctx2.Entry(line1).State}");


                ctx2.SaveChanges();
            }

        }
    }
}
