using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp
{

    public class ShopCategory 
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public class User 
    {
        public string userName { get; set; }
        public string password { get; set; }
        public enum userType
        {
            ADMIN, CUSTOMER
        }
        public userType type;
        public List<Order> orderHistory { get; set; } = new List<Order>();
    }
    public class ShopProduct : INotifyPropertyChanged 
    {
        public ShopCategory Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string priceString { get; private set; }
        public double Tax { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private double _Price;
        public double Price
        {
            get
            {
                return _Price;
            }
            set
            {
                _Price = value;
                priceString = string.Format("${0:N2}", value);
            }
        }

        public string TotalSalesString { get; private set; } = "$0.00";
        public double totalUnitsSold { get; set; } = 0;
        private double _totalSales=0;
        public double TotalSales
        {
            get
            {
                return _totalSales;
            }
            set
            {
                _totalSales = value;
                TotalSalesString = string.Format("${0:N2}", value);
            }

        }

        private uint _stockQuantity;
        public uint StockQuantity
        {
            get { return _stockQuantity; }
            set
            {
                _stockQuantity = value;
                OnPropertyChanged();
            }
        }
        public override string ToString()
        {
            return Name;
        }

    }
    public class cartItem : INotifyPropertyChanged 
    {
        public ShopProduct Product { get; set; }
        public string RateString { get; private set; }
        public double Rate { get; private set; }
        public string TaxString { get; private set; }
        public string TotalString { get; private set; }
        public double Tax { get; private set; }
        private uint _qty = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public uint Qty
        {
            get { return _qty; }
            set
            {
                _qty = value; Rate = Product.Price * _qty;
                RateString = string.Format("${0:N2}", Rate);
                Tax = Rate * Product.Tax / 100;
                TaxString = string.Format("${0:N2}", Tax);
                TotalString = string.Format("${0:N2}", Tax + Rate);
                OnPropertyChanged();
                OnPropertyChanged("RateString");
                OnPropertyChanged("TaxString");
                OnPropertyChanged("TotalString");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Order // Naman
    {
        public List<cartItem> cartItems { get; set; } = new List<cartItem>();
        public DateTime orderTime { get; set; }
        public string totalAmount { get; set; }
    }
}
