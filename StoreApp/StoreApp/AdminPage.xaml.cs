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
using System.Text.RegularExpressions;

namespace StoreApp
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        User user;
        List<ShopProduct> selectedCategoryProducts = new List<ShopProduct>();//Categorized
        List<ShopProduct> searchedProducts = new List<ShopProduct>();//Filtered / Searched Products
        List<ShopProduct> selectedProductsSorted;//Sorted Products

        List<ShopCategory> selectedCategory = new List<ShopCategory>();

        double totalAmount;
        string searchText { get; set; } = "";
        public AdminPage(User curUser)
        {
            user = curUser;
            InitializeComponent();
            totalAmount = 0;

            selectedCategory.AddRange(publicData.myCategory);//Select all Categories
            for (int i = 0; i < publicData.myProducts.Count; i++)
            {
                totalAmount += publicData.myProducts[i].TotalSales;//Calculate total Sales
            }
            txtSalesAmount.Text = string.Format("${0:N2}", totalAmount);
            searchUpdate();//Update the List
            lstCategory.ItemsSource = publicData.myCategory;
        }
        private void searchUpdate()
        {
            searchedProducts.Clear();
            for (int i = 0; i < selectedCategoryProducts.Count; i++)
            {
                if (selectedCategoryProducts[i].ToString().ToLower().Contains(searchText))
                {
                    searchedProducts.Add(selectedCategoryProducts[i]);//Filter Search Test
                }
            }
            selectedProductsSorted = searchedProducts.OrderBy(o => o.Name).ToList();//Sort List
            lstProduct.ItemsSource = null;
            lstProduct.ItemsSource = selectedProductsSorted;//Refresh List

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClear_Click_1(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            searchText = txtSearch.Text.ToLower();
            if (searchText == "") btnClear.IsEnabled = false;
            else btnClear.IsEnabled = true;
            searchUpdate();//Search unicase
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.GoBack();
        }

        private void chkCategory_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox curCheck = (CheckBox)sender;
            ShopCategory curCategory = (ShopCategory)(curCheck).Tag;
            for (int i = 0; i < publicData.myProducts.Count; i++)
            {
                if (publicData.myProducts[i].Category == curCategory)
                {
                    selectedCategoryProducts.Add(publicData.myProducts[i]);//Add Selected Category
                }
            }
            selectedCategory.Add(curCategory);
            searchUpdate();
        }

        private void chkCategory_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox curCheck = (CheckBox)sender;
            ShopCategory curCategory = (ShopCategory)(curCheck).Tag;
            for (int i = 0; i < selectedCategoryProducts.Count; i++)
            {
                if (selectedCategoryProducts[i].Category == curCategory)
                {
                    selectedCategoryProducts.RemoveAt(i--);//Remove Unchecked Category
                }
            }
            selectedCategory.Remove(curCategory);
            searchUpdate();
        }

        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            EditProduct newPage = new EditProduct((ShopProduct)((Button)sender).Tag);
            this.NavigationService.Navigate(newPage);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);//Only allow Numeric Values in Order Quantity
        }
        private void btnAddStock_Click(object sender, RoutedEventArgs e)
        {

            //Bring up Order Stock Popup
            popupOrder.PlacementTarget = (Button)sender;
            txtOrderStock.Text = "";
            popupOrder.IsOpen = true;
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            //Add Stock Quantity
            ((ShopProduct)((Button)popupOrder.PlacementTarget).Tag).StockQuantity += Convert.ToUInt32(txtOrderStock.Text);
            popupOrder.IsOpen = false;
        }

        private void btnOrderHistory_Click(object sender, RoutedEventArgs e)
        {
            OrderHistory newPage = new OrderHistory();
            this.NavigationService.Navigate(newPage);
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            EditProduct newPage = new EditProduct();
            this.NavigationService.Navigate(newPage);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            selectedCategoryProducts.Clear();
            for (int i = 0; i < publicData.myProducts.Count; i++)
            {
                if (selectedCategory.Contains(publicData.myProducts[i].Category))
                {
                    selectedCategoryProducts.Add(publicData.myProducts[i]);//Update Data
                }
            }
            searchUpdate();
        }
    }
}
