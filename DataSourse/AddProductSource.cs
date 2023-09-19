using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Shared.PlatformSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MP4practic.Context;
using MP4practic.Models;
using Tmds.DBus;

namespace MP4practic.DataSourse
{
    public class AddProductSource
    {
        Avalonia.Media.Imaging.Bitmap image;
        private string productArticleNumber = null!;
        private string productName = null!;
        private string productDescription = null!;
        private int productCategory = 0;
        private string productPhoto = null!;
        private string productManufacturer = null!;
        private int productDelivery = 0!;
        private decimal productCost = 0!;
        private int productDiscountAmount = 0!;
        private int productQuantityInStock = 0!;
        private int productUnitOfMeasurement = 0!;
        private int productSall = 0!;
        public int ProductSall
        {
            get
            {
                return productSall;
            }
            set
            {
                productSall = value;
                NotifyPropertyChanged();
            }
        }

        public string ProductArticleNumber
        {
            get
            {
                return productArticleNumber;
            }
            set
            { productArticleNumber = value; 
                NotifyPropertyChanged();
            }
        }
        public string ProductName
        {
            get
            {
                return productName;
            }
            set
            {
                productName = value;
                NotifyPropertyChanged();
            }
        }
        public string ProductDescription
        {
            get
            {
                return productDescription;
            }
            set
            {
                productDescription = value;
                NotifyPropertyChanged();
            }
        }
        public int ProductCategory
        {
            get
            {
                return productCategory;
            }
            set
            {
                productCategory = value;
                NotifyPropertyChanged();
            }
        }
        public string ProductPhoto
        {
            get
            {
                return productPhoto;
            }
            set
            {
                productPhoto = value;
                NotifyPropertyChanged();
            }
        }
        public string ProductManufacturer
        {
            get
            {
                return productManufacturer;
            }
            set
            {
                productManufacturer = value;
                NotifyPropertyChanged();
            }
        }
        public int ProductDelivery
        {
            get
            {
                return productDelivery;
            }
            set
            {
                productDelivery = value;
                NotifyPropertyChanged();
            }
        }
        public decimal ProductCost
        {
            get
            {
                return productCost;
            }
            set
            {
                productCost = value;
                NotifyPropertyChanged();
            }
        }
        public int ProductDiscountAmount
        {
            get
            {
                return productDiscountAmount;
            }
            set
            {
                productDiscountAmount = value;
                NotifyPropertyChanged();
            }
        }
        public int ProductQuantityInStock
        {
            get
            {
                return productQuantityInStock;
            }
            set
            {
                productQuantityInStock = value;
                NotifyPropertyChanged();
            }
        }
        public int ProductUnitOfMeasurement
        {
            get
            {
                return productUnitOfMeasurement;
            }
            set
            {
                productUnitOfMeasurement = value;
                NotifyPropertyChanged();
            }
        }
        public AddProductSource() {


           
        }

        public PostgresContext context;
        public AddProductSource(Product product)
        {
            context = PostgresContext.GetContext();
            ProductArticleNumber = product.Productarticlenumber;
            ProductName = product.Productname;
            ProductCategory = product.Productcategory;
            ProductPhoto = product.Productphoto;
            ProductCost = product.Productcost;
            ProductDelivery = product.Productdelivery;
            ProductDescription = product.Productdescription;
            ProductManufacturer = context.Manufacturerproducts.Where(x=> x.Manufacturerid == product.Productmanufacturer).First().Manufacturername;
            ProductDiscountAmount = product.Productdiscountamount;
            ProductQuantityInStock = product.Productquantityinstock;
            ProductUnitOfMeasurement = product.Productunitofmeasurement;
            /*ProductSall = product.Productsall;*/

        }
        public void CloseAdd()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Windows.OfType<AddOrRedactProduct>().First().Close();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertuName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertuName));

        }
    }
}
