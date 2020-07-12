using System.Collections.Generic;

namespace StoreApp
{
    static class publicData 
    {

        public static List<ShopCategory> myCategory = new List<ShopCategory>
        {
            new ShopCategory{Name="Stationary"},
            new ShopCategory{Name="Electronics"},
            new ShopCategory{Name="Accessories"},
            new ShopCategory{Name="Sports"},
            new ShopCategory{Name="Software"},
        };

        public static List<ShopProduct> myProducts = new List<ShopProduct>
        {
            new ShopProduct{Category=myCategory[0],Tax=15.0,Description="210*297 mm Paper Bundle",Name="A4 Paper Bundle",Price=20.00,StockQuantity=10},
            new ShopProduct{Category=myCategory[0],Tax=15.0,Description="Set of 10 pens",Name="Pen Set",Price=2.00,StockQuantity=20},
            new ShopProduct{Category=myCategory[1],Tax=15.0,Description="The Laptop for the Developers",Name="Macbook pro",Price=1299.00,StockQuantity=1},
            new ShopProduct{Category=myCategory[1],Tax=15.0,Description="Apple iPhone XR smartphone.",Name="Iphone XR",Price=999.00,StockQuantity=2},
            new ShopProduct{Category=myCategory[2],Tax=15.0,Description="The Best Bass In Town",Name="Earphones",Price=20.00,StockQuantity=5},
            new ShopProduct{Category=myCategory[2],Tax=15.0,Description="Battery Backup All Day",Name="Power Bank",Price=3.00,StockQuantity=3},
            new ShopProduct{Category=myCategory[3],Tax=15.0,Description="For the Game Lovers",Name="Socce Ball Set",Price=44.49,StockQuantity=3},
            new ShopProduct{Category=myCategory[4],Tax=15.0,Description="The Genuine Windows 10 with pen Drive",Name="Windows 10 Professional",Price=138.00,StockQuantity=3},
        };

        public static List<User> myUser = new List<User>
        {
            new User{userName="UserXYZ",password="myPassXYZ", type=User.userType.CUSTOMER},
            new User{userName="UserABC",password="myPassABC", type=User.userType.CUSTOMER},
            new User{userName="Admin",password="Password", type=User.userType.ADMIN},
        };

    }
}
