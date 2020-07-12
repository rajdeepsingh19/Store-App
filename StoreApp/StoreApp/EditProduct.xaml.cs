using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoreApp
{
    /// <summary>
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Page
    {
        private ShopProduct _editProduct;
        public EditProduct()
        {
            InitializeComponent();
            cmbCategory.ItemsSource = publicData.myCategory;
            btnSave.Content = "Add New";
            cmbCategory.SelectedIndex = 0;
        }
        public EditProduct(ShopProduct EditProduct)
        {
            InitializeComponent();
            cmbCategory.ItemsSource = publicData.myCategory;
            _editProduct = EditProduct;

            cmbCategory.SelectedItem = _editProduct.Category;
            txtName.Text = _editProduct.Name;
            txtPrice.Text = string.Format("{0:N2}", _editProduct.Price);
            txtTax.Text = string.Format("{0:N2}", _editProduct.Tax);
            txtDesc.Text = _editProduct.Description;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ShopCategory ct = (ShopCategory)cmbCategory.SelectedItem;

            //Check Data 
            if (Convert.ToDouble(txtPrice.Text)==0)
            {
                MessageBox.Show("Price cannot be Zero");
                txtPrice.Focus();
                return;
            }
            if (txtName.Text=="")
            {
                MessageBox.Show("Please enter Name");
                txtName.Focus();
                return;
            }

            if (_editProduct==null)//New Product
            {
                _editProduct = new ShopProduct();
                _editProduct.Category = ct;
                _editProduct.Name = txtName.Text;
                _editProduct.Price = Convert.ToDouble(txtPrice.Text);
                _editProduct.Tax=Convert.ToDouble(txtTax.Text);
                _editProduct.Description = txtDesc.Text;
                publicData.myProducts.Add(_editProduct);
            }
            else //Edit Product
            {
                _editProduct.Category = ct;
                _editProduct.Name = txtName.Text;
                _editProduct.Price = Convert.ToDouble(txtPrice.Text);
                _editProduct.Tax = Convert.ToDouble(txtTax.Text);
                _editProduct.Description = txtDesc.Text;
            }
            this.NavigationService.GoBack();
        }

        private void txtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d*[\.]?[\d]?[\d]?$");//Regex To Match Format of Currency
            e.Handled = !regex.IsMatch(txtPrice.Text.Insert(txtPrice.CaretIndex, e.Text));
        }

        private void txtTax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[\d]?[\d]?[\.]?[\d]?[\d]?$");//Regex To Match Format of xx.xx
            e.Handled = !regex.IsMatch(txtTax.Text.Insert(txtTax.CaretIndex, e.Text));
        }
    }
}
