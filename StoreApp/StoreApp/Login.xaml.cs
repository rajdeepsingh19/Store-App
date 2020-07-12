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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            User loggedUser = null;
            foreach (User item in publicData.myUser)
            {
                //Check if username exists - Case insensitive
                if (string.Equals(txtName.Text.ToLower(),item.userName.ToLower()))
                {

                    if (string.Equals(txtPass.Password, item.password))
                    {
                        loggedUser = item;//Login Success
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Username and Password Doesnt Match");
                        break;
                    }
                }
            }
            if (loggedUser==null)
            {
                MessageBox.Show("User doesnt Exist");//Login Failed
            }
            else
            {
                //Transfer to appropriate pages
                if (loggedUser.type == User.userType.CUSTOMER)
                {
                    Store newPage = new Store(loggedUser);
                    txtPass.Clear();
                    txtName.Clear();
                    this.NavigationService.Navigate(newPage);
                }
                else//ADMIN
                {
                    AdminPage newPage = new AdminPage(loggedUser);
                    txtPass.Clear();
                    txtName.Clear();
                    this.NavigationService.Navigate(newPage);
                }
            }
        }
    }
}
