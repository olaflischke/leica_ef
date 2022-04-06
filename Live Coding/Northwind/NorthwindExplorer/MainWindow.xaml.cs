﻿using NorthwindDal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NorthwindExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NorthwindContext context = new NorthwindContext();

        public MainWindow()
        {
            InitializeComponent();

            context.Database.Log = LogIt;

            // Sample Code für LINQ ohne(!) DB
            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\tmp");

            var files = from fi in directoryInfo.EnumerateFiles()
                        where fi.Name.StartsWith("b")
                        select new { FileName = fi.Name, Path = fi.FullName };


            // select distinct country from Customers
            // Deklarativ:
            var qCustomers = from cu in context.Customers
                             select cu; //.Country).Distinct();

            var qCountries = qCustomers.Select(cu => cu.Country).Distinct();

            // Lambda:
            var qCountries2 = context.Customers.Select(cu => cu.Country).Distinct();


            foreach (string country in qCountries)
            {
                TreeViewItem tviLand = new TreeViewItem() { Header = country };
                tviLand.Items.Add(new TreeViewItem());
                tviLand.Expanded += this.TviLand_Expanded;
                trvCustomers.Items.Add(tviLand);
            }
        }

        private void TviLand_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem tviLand)
            {
                tviLand.Items.Clear();

                string country = tviLand.Header.ToString();

                var qCustomersFromCountry = context.Customers
                                                    .Where(cu => cu.Country == country)
                                                    .Select(cu => new { cu.CustomerID, cu.CompanyName });
                //.Select(cu => new TreeViewItem() { Header = cu.CompanyName, Tag = cu.CustomerID });

                var qCustomersFromCountry2 = context.Customers.AsEnumerable()
                                    .Where(cu => CheckCountry(cu, country));


                //tviLand.ItemsSource = qCustomersFromCountry.ToList();

                foreach (var customer in qCustomersFromCountry2)
                {
                    TreeViewItem tviCustomer = new TreeViewItem() { Header = customer.CompanyName, Tag = customer.CustomerID };
                    tviCustomer.Selected += this.TviCustomer_Selected;
                    tviLand.Items.Add(tviCustomer);
                }
            }
        }

        private bool CheckCountry(Customer cu, string country)
        {
            if (cu.Country == country)
            {
                return true;
            }
            return false;
        }

        private void TviCustomer_Selected(object sender, RoutedEventArgs e)
        {
            string customerId = ((TreeViewItem)sender).Tag.ToString();
            //Customer customer = context.Customers.Find(customerId);
            //cbxOrders.ItemsSource = customer?.Orders.ToList();

            var qOrdersOfCustomer = context.Orders.Where(od => od.CustomerID == customerId).Select(od => od.ID);
            cbxOrders.ItemsSource = qOrdersOfCustomer.ToList();
        }

        private void LogIt(string logString)
        {
            txtLog.Text += logString;
            txtLog.ScrollToEnd();
        }

        private void cbxOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int orderID = Convert.ToInt32(cbxOrders.SelectedItem);

            // Quantity, Productname, UnitPrice, Discount - Navigationeigenschaften nutzen!
            var qOrderInfo = context.Order_Details
                                                .Where(od => od.OrderID == orderID)
                                                .Select(od => new { od.Quantity, od.Product.ProductName, od.UnitPrice, od.Discount });

            // Join in LINQ
            var qOrderInfo2 = from od in context.Order_Details
                              join pd in context.Products on od.ProductID equals pd.ID
                              where od.OrderID == orderID
                              select new { od.Quantity, pd.ProductName, od.UnitPrice, od.Discount };

            dgOrderInfo.ItemsSource = qOrderInfo.ToList();
        }
    }
}
