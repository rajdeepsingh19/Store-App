using System;
using System.Collections.Generic;
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

namespace StoreApp
{
    /// <summary>
    /// Interaction logic for OrderHistory.xaml
    /// </summary>
    public partial class OrderHistory : Page
    {
        public List<User> listUsers = new List<User>();
        public OrderHistory()
        {
            InitializeComponent();


            //Check the users for OrderHistory. Only add if theres an order in his profile
            foreach (User item in publicData.myUser)
            {
                if (item.orderHistory.Count>0)
                {
                    listUsers.Add(item);
                }
            }

            if (listUsers.Count==0)//No Orders
            {
                TextBlock tb = new TextBlock();
                tb.Text = "NO RECENT ORDERS";
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.FontSize = 20;
                mainScroll.Content = tb;
            }
            lstCustomer.ItemsSource = listUsers;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
