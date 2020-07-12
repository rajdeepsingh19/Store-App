using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StoreApp
{
    /// <summary>
    /// Interaction logic for Store.xaml
    /// </summary>
    public partial class Store : Page
    {

        List<cartItem> myCart = new List<cartItem>();
        List<ShopProduct> selectedCategoryProducts = new List<ShopProduct>();//Categorized
        List<ShopProduct> searchedProducts = new List<ShopProduct>();//Filtered / Searched Prody
        List<ShopProduct> selectedProductsSorted;//Filtered / Searched Prody
        List<ShopProduct> selectedProducts = new List<ShopProduct>();//Cart Products
        DispatcherTimer dtClockTime = new DispatcherTimer();
        string searchText="";
        double totalAmount;
        double totalTax;
        User user;
        public Store(User curUser)
        {
            InitializeComponent();
            for (int i = 0; i < publicData.myProducts.Count; i++)
            {
                selectedCategoryProducts.Add(publicData.myProducts[i]);
            }
            user = curUser;
            searchUpdate();
            lstCart.ItemsSource = myCart;
            lstCategory.ItemsSource = publicData.myCategory;
            dtClockTime.Interval = new TimeSpan(0, 0, 1); //in Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick; // For Closing Notification Popup automatically
            lstCartHistory.ItemsSource = user.orderHistory;
        }
        private void searchUpdate()
        {
            searchedProducts.Clear();
            for (int i = 0; i < selectedCategoryProducts.Count; i++)
            {
                if (selectedCategoryProducts[i].ToString().ToLower().Contains(searchText))//Make sure search text is in product name
                {
                    if (selectedCategoryProducts[i].StockQuantity>0) searchedProducts.Add(selectedCategoryProducts[i]);
                }
            }
            selectedProductsSorted = searchedProducts.OrderBy(o => o.Name).ToList();//Sort List
            lstProduct.ItemsSource = null;
            lstProduct.ItemsSource = selectedProductsSorted;//Refresh Display

        }
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            cartPopupNotify.IsOpen = false;//Close Notification Popup Automatically
            dtClockTime.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShopProduct curProd = (ShopProduct)((Button)sender).Tag;
            if (selectedProducts.Contains(curProd))//Check if product is already in cart
            {
                cartNotification.Text = "Already in Cart";
                cartNotificationBG.Color = Colors.Cyan;
            }
            else
            {
                //Add to Cart
                cartItem newItem = new cartItem();
                newItem.Product = curProd;
                newItem.Qty = 1;
                selectedProducts.Add(curProd);
                myCart.Add(newItem);
                cartNotification.Text = "Added to Cart";
                cartNotificationBG.Color = Colors.Yellow;
                btnCheckout.IsEnabled = true;
                btnClearCart.IsEnabled = true;
                cartHeader.Visibility = Visibility.Visible;


                //Update Cart Amount
                totalAmount = 0;
                totalTax = 0;
                for (int i = 0; i < myCart.Count; i++)
                {
                    totalAmount += myCart[i].Rate;
                    totalTax += myCart[i].Tax;
                }
                lbltax.Text = string.Format("${0:N2}", totalTax);
                lblsubAmount.Text = string.Format("${0:N2}", totalAmount);
                lbltotal.Text = string.Format("${0:N2}", totalAmount+totalTax);
            }

            cartPopupNotify.IsOpen = true;//Notify
            dtClockTime.Stop();
            dtClockTime.Start();
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            lstCart.ItemsSource = null;
            lstCart.ItemsSource = myCart;
            cartPopup.IsOpen = true;
        }

        private void qty_sub_Click(object sender, RoutedEventArgs e)
        {
            //Reduce Product Quantity

            cartItem curProd = (cartItem)((Button)sender).Tag;
            curProd.Qty--;
            if (curProd.Qty==0)
            {
                myCart.Remove(curProd);
                lstCart.ItemsSource = null;
                lstCart.ItemsSource = myCart;
                if (myCart.Count==0)
                {
                    btnCheckout.IsEnabled = false;
                    btnClearCart.IsEnabled = false;
                    selectedProducts.Remove(curProd.Product);
                    lbltax.Text = "$0.00";
                    lblsubAmount.Text = "$0.00";
                    lbltotal.Text = "$0.00";
                    cartHeader.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                totalAmount = 0;
                totalTax = 0;
                for (int i = 0; i < myCart.Count; i++)
                {
                    totalAmount += myCart[i].Rate;
                    totalTax += myCart[i].Tax;
                }
                lbltax.Text = string.Format("${0:N2}", totalTax);
                lblsubAmount.Text = string.Format("${0:N2}", totalAmount);
                lbltotal.Text = string.Format("${0:N2}", totalAmount + totalTax);
            }
        }

        private void qty_add_Click(object sender, RoutedEventArgs e)
        {
            //Increase Product Quantity
            cartItem curProd = (cartItem)((Button)sender).Tag;
            if (curProd.Qty<curProd.Product.StockQuantity) curProd.Qty++;

            totalAmount = 0;
            totalTax = 0;
            for (int i = 0; i < myCart.Count; i++)
            {
                totalAmount += myCart[i].Rate;
                totalTax += myCart[i].Tax;
            }
            lbltax.Text = string.Format("${0:N2}", totalTax);
            lblsubAmount.Text = string.Format("${0:N2}", totalAmount);
            lbltotal.Text = string.Format("${0:N2}", totalAmount + totalTax);

        }

        private void btnClearCart_Click(object sender, RoutedEventArgs e)
        {

            //Clear Cart
            myCart.Clear();
            lstCart.ItemsSource = null;
            lstCart.ItemsSource = myCart;
            btnCheckout.IsEnabled = false;
            btnClearCart.IsEnabled = false;
            cartHeader.Visibility = Visibility.Collapsed;
            selectedProducts.Clear();
            lbltax.Text = "$0.00";
            lblsubAmount.Text = "$0.00";
            lbltotal.Text = "$0.00";
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            Order newOrder = new Order();
            totalAmount = 0;
            totalTax = 0;
            foreach(cartItem item in myCart)
            {
                item.Product.StockQuantity -= item.Qty;//Reduce from Stock
                item.Product.totalUnitsSold += item.Qty;
                //Update Sales Amount
                item.Product.TotalSales += item.Rate + item.Tax;
                totalAmount += item.Rate;
                totalTax += item.Tax;
            }
            //Update order history from Cart
            newOrder.cartItems = myCart;
            newOrder.orderTime = DateTime.Now;
            newOrder.totalAmount = string.Format("${0:N2}", totalAmount + totalTax);
            totalAmount = 0;
            totalTax = 0;
            user.orderHistory.Add(newOrder);
            myCart=new List<cartItem>();//Clear Cart
            lstCart.ItemsSource = null;
            lstCart.ItemsSource = myCart;
            btnCheckout.IsEnabled = false;
            btnClearCart.IsEnabled = false;
            cartHeader.Visibility = Visibility.Collapsed;
            lbltax.Text = "$0.00";
            lblsubAmount.Text = "$0.00";
            lbltotal.Text = "$0.00";
            lstCartHistory.ItemsSource = null;
            lstCartHistory.ItemsSource = user.orderHistory;
            selectedProducts.Clear();
            searchUpdate();
            cartPopup.IsOpen = false;
            cartNotification.Text = "Checked Out";
            cartNotificationBG.Color = Colors.LightGreen;
            cartPopupNotify.IsOpen = true;//Notify
            dtClockTime.Stop();
            dtClockTime.Start();
        }

        private void chkCategory_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox curCheck = (CheckBox)sender;
            ShopCategory curCategory = (ShopCategory)(curCheck).Tag;
            for (int i = 0; i < publicData.myProducts.Count; i++)
            {
                if (publicData.myProducts[i].Category == curCategory)
                {
                    selectedCategoryProducts.Add(publicData.myProducts[i]);//Add Checked Category
                }
            }
            searchUpdate();
        }

        private void chkCategory_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox curCheck = (CheckBox)sender;
            ShopCategory curCategory = (ShopCategory)(curCheck).Tag;
            for (int i = 0; i < selectedCategoryProducts.Count; i++)
            {
                if (selectedCategoryProducts[i].Category==curCategory)
                {
                    selectedCategoryProducts.RemoveAt(i--);//Remove UnChecked Category
                }
            }
            searchUpdate();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            btnClear.IsEnabled = false;
            searchText = "";
            txtSearch.Text = "";
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Update product list based on search Text
            searchText = txtSearch.Text.ToLower();
            if (searchText == "") btnClear.IsEnabled = false;
            else btnClear.IsEnabled = true;
            searchUpdate();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            orderHistoryPopup.IsOpen = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
