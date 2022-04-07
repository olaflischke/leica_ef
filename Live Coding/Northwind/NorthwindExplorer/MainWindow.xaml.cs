using NorthwindDal;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        //NorthwindContext context = new NorthwindContext();

        public MainWindow()
        {
            InitializeComponent();

            NorthwindContext context = InitializeContext();

            // Sample Code für LINQ ohne(!) DB
            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\tmp");

            var files = from fi in directoryInfo.EnumerateFiles()
                        where fi.Name.StartsWith("b")
                        select new { FileName = fi.Name, Path = fi.FullName };


            // select distinct country from Customers
            // Deklarativ:
            var qCustomers = from cu in context.Customers.AsNoTracking()
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

        private NorthwindContext InitializeContext()
        {
            NorthwindContext context = new NorthwindContext();
            context.Database.Log = LogIt;
            return context;
        }

        private void TviLand_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem tviLand)
            {
                tviLand.Items.Clear();

                string country = tviLand.Header.ToString();

                NorthwindContext context = InitializeContext();

                var qCustomersFromCountry = context.Customers
                                                    .Where(cu => cu.Country == country)
                                                    .Select(cu => new { cu.CustomerID, cu.CompanyName });
                //.Select(cu => new TreeViewItem() { Header = cu.CompanyName, Tag = cu.CustomerID });

                var qCustomersFromCountry2 = context.Customers.AsNoTracking().AsEnumerable()
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

            NorthwindContext context = InitializeContext();

            var qOrdersOfCustomer = context.Orders.AsNoTracking().Where(od => od.CustomerID == customerId).Select(od => od.ID);
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

            NorthwindContext context = InitializeContext();


            // Quantity, Productname, UnitPrice, Discount - Navigationeigenschaften nutzen!
            var qOrderInfo = context.Order_Details.AsNoTracking()
                                                .Where(od => od.OrderID == orderID)
                                                .Select(od => new { od.Quantity, od.Product.ProductName, od.UnitPrice, od.Discount });

            // Join in LINQ
            var qOrderInfo2 = from od in context.Order_Details
                              join pd in context.Products on od.ProductID equals pd.ID
                              where od.OrderID == orderID
                              select new { od.Quantity, pd.ProductName, od.UnitPrice, od.Discount };

            dgOrderInfo.ItemsSource = qOrderInfo.ToList();
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer(); // ist dem Context unbekannt!
            customer.Country = "Germany";

            AddEditCustomer dlgAddCustomer = new AddEditCustomer(customer);

            if (dlgAddCustomer.ShowDialog() == true)
            {
                NorthwindContext context = InitializeContext();

                context.Customers.Add(customer); // dem Context bekantmachen
                context.SaveChanges();
            }
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            string customerId = ((TreeViewItem)trvCustomers.SelectedItem).Tag?.ToString();

            if (string.IsNullOrEmpty(customerId))
            {
                return;
            }
            Customer customer = null;
            using (NorthwindContext context1 = InitializeContext())
            {
                customer = context1.Customers.Find(customerId);

                if (customer != null)
                {
                    AddEditCustomer dlgEditCustomer = new AddEditCustomer(customer);
                    if (dlgEditCustomer.ShowDialog() == true)
                    {
                        try
                        {
                            //context1.Customers.Attach(customer);
                            //context1.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                            context1.SaveChanges();

                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            MessageBox.Show("Fehler: Daten in der Datenbank sind neuer als deine!");

                            // Client wins
                            //context.Entry(customer).OriginalValues.SetValues(context.Entry(customer).GetDatabaseValues());
                            //context.SaveChanges();

                            // Database wins
                            //context.Entry(customer).Reload();

                        }
                    }
                    else
                    {
                        //context.Entry(customer).Reload();

                        ////context.Entry(customer).CurrentValues.SetValues(context.Entry(customer).OriginalValues);
                        //context.Entry(customer).CurrentValues.SetValues(context.Entry(customer).GetDatabaseValues());
                        //context.Entry(customer).State = System.Data.Entity.EntityState.Unchanged;
                    }
                }
            }
        }
    }
}
