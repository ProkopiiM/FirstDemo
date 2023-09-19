using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Shared.PlatformSupport;
using MP4practic.Context;
using MP4practic.Models;
using MP4practic.ModelsDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MessageBox.Avalonia;
using MP4practic.DataSourse;

namespace MP4practic.Views
{
    public partial class LobbyApp : Window
    {
        public static PostgresContext context;
        
        public LobbyApp()
        {
            InitializeComponent();
            DataContext = new LobbyModel();
            context = PostgresContext.GetContext();
            ComboProizvoditel.SelectionChanged += Vibor;
            LoadProduct();
            ListBoxItems.IsVisible = false;
            ListBoxItemsUser.IsVisible = true;
            Proizvoditeli();
        }

        public void Proizvoditeli()
        {
            List<Proizvod> p = new List<Proizvod>();
            p.Add(new Proizvod(){Manufacturername = "Все производители"});
            foreach (var VARIABLE in context.Manufacturerproducts)
            {
                Proizvod pr = new Proizvod();
                pr.Manufacturerid = VARIABLE.Manufacturerid;
                pr.Manufacturername = VARIABLE.Manufacturername;
                p.Add(pr);
            }
            ComboProizvoditel.Items = p;
        }
        private void Vibor(object? sender, SelectionChangedEventArgs e)
        {
            ListBoxItems.Items = null;
            ListBoxItemsUser.Items = null;
            listProducts.Clear();
            LoadProduct();
        }
        List<Product> products = new List<Product>();
        public LobbyApp(string loginpassword,int roleid)
        {
            InitializeComponent();
            DataContext = new LobbyModel();
            context = PostgresContext.GetContext();
            
            ComboProizvoditel.SelectionChanged += Vibor;
            Proizvoditeli();
            
            if(roleid == 1)
            {
                AddProduct.IsVisible = true;
            }
            loginPassword.Text = loginpassword.Split("_")[0].ToString() + " " + loginpassword.Split("_")[1].ToString();
            LoadProduct();
        }
        
        public List<ListProduct> listProducts = new List<ListProduct>();
        
        public void LoadProduct()
        { 
            products = context.Products.ToList();
            
            listProducts.Clear();
            if (SearchBox.Text != null && SearchBox.Text != "" && SearchBox.Text.Split(' ').Length == 1)
            {
                this.products = this.products.Where(x =>
                    x.Productname.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    x.Productdescription.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    x.ProductmanufacturerNavigation.Manufacturername.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    (Convert.ToDouble(x.Productcost) - ((Convert.ToDouble(x.Productcost) * Convert.ToDouble(x.Productsall / 100)))).ToString().Contains(SearchBox.Text)).ToList();
            }
            else if (SearchBox.Text != null && SearchBox.Text != "" && SearchBox.Text.Split(' ').Length > 1)
            {
                var list = SearchBox.Text.Split(' ');
                var prod = new List<Product>();
                List<Product> pr = new List<Product>();
                for(int i = 0; i < list.Length; i++)
                {
                    if (prod.Count == 0)
                    {
                        pr = this.products.Where(x =>
                            x.Productname.ToLower().Contains(list[i]) ||
                            x.Productdescription.ToLower().Contains(list[i]) ||
                            x.ProductmanufacturerNavigation.Manufacturername.ToLower().Contains(list[i]) ||
                            x.Productcost.ToString()
                                .Contains(list[i])).ToList();
                    }
                    else if (prod.Count > 0)
                    {
                        pr = prod.Where(x =>
                            x.Productname.ToLower().Contains(list[i]) ||
                            x.Productdescription.ToLower().Contains(list[i]) ||
                            x.ProductmanufacturerNavigation.Manufacturername.ToLower().Contains(list[i]) ||
                            x.Productcost.ToString().Contains(list[i])).ToList();
                        prod.Clear();
                    }
                    foreach (var VARIABLE in pr)
                    {
                        prod.Add(VARIABLE);
                    }
                }
                products = prod;
            }
            if (ComboProizvoditel.SelectedIndex > 0)
            {
                this.products = this.products
                    .Where(x => x.Productmanufacturer == ComboProizvoditel.SelectedIndex).ToList();
            }
            if (ComboUpDown.SelectedIndex != 0)
            {
                this.products = this.products.OrderBy(x => x.Productcost).ToList();
            }
            if (ComboUpDown.SelectedIndex != 1)
            {
                this.products = this.products.OrderByDescending(x => x.Productcost).ToList();
            }
            var manufacturerproducts = context.Manufacturerproducts.ToList(); 
            AssetLoader assetLoader = new AssetLoader();

            foreach (var product in products)
            {
                ListProduct productList = new ListProduct();
                try
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProducts");
                    imagePath = imagePath.Replace("bin\\Debug\\net7.0\\", "");
                    Bitmap image = new Bitmap(Path.Combine(imagePath,product.Productphoto));
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        image.Save(memoryStream);
                    }
                    productList.ImageProduct = image;
                }
                catch (Exception ex)
                {
                    productList.ImageProduct = new Avalonia.Media.Imaging.Bitmap(assetLoader.Open(new Uri("avares://MP4practic/ImageProducts/picture.png")));
                }
                productList.ProductName = product.Productname;
                productList.ManufacturerName = manufacturerproducts.Where(x => product.Productmanufacturer == x.Manufacturerid).First().Manufacturername;
                productList.ProductDescription = product.Productdescription;
                productList.ProductQuantityInStock = product.Productquantityinstock;
                productList.ProductManufacturer = product.Productmanufacturer;
                /*double o = product.Productsall;
                o = o / 100;
                Math.Round(o, 3);
                double p = Decimal.ToDouble(product.Productcost);
                var t = p - (p * o);*/
                productList.ProductCost = product.Productcost; /*Math.Round(Convert.ToDecimal(t),3)*/
                productList.Productarticlenumber = product.Productarticlenumber;
                listProducts.Add(productList);
            }

            ListBoxItemsUser.Items = null;
            ListBoxItems.Items = null;
            ListBoxItemsUser.Items = listProducts;
            ListBoxItems.Items = listProducts;
            countItems.Text = listProducts.Count.ToString() + "/" + context.Products.Count().ToString();
        }
        private async void Redact_OnClick(object? sender, RoutedEventArgs e)
        {
            string bin = (string)(sender as Button).Tag;
            var product = context.Products.Where(x => x.Productarticlenumber == bin).First();
            if (product != null)
            {
                AddOrRedactProduct addOrRedactProduct = new AddOrRedactProduct(product);
                await addOrRedactProduct.ShowDialog(this);
            }
            UpdateListProduct();
            Proizvoditeli();
        }

        public void UpdateListProduct()
        {
            ListBoxItems.Items = null;
            ListBoxItemsUser.Items = null;
            listProducts.Clear();
            LoadProduct();
        }
        private void UpdateList_OnClick(object? sender, RoutedEventArgs e)
        {
            ListBoxItems.Items = null;
            ListBoxItemsUser.Items = null;
            listProducts.Clear();
            LoadProduct();
        }

        private async void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            string bin = (string)(sender as Button).Tag;
            var product = context.Products.Where(x => x.Productarticlenumber == bin).First();
            try
            {
                context.Remove(product);
                context.SaveChanges();
                var mess = MessageBoxManager.GetMessageBoxStandardWindow("Удаление товара", "Продукт удален");
                await mess.ShowDialog(this);
                UpdateListProduct();
                Proizvoditeli();
            }
            catch (Exception exception)
            {
                var mess = MessageBoxManager.GetMessageBoxStandardWindow("Удаление товара", "Продукт неудален");
                mess.ShowDialog(this);
            }
        }

        private void SearchBox_OnKeyUp(object? sender, KeyEventArgs e)
        {
            /*var spl = SearchBox.Text.Split(" ");
            foreach (var a in spl)
            {
                if (SearchBox.Text != null)
                {
                    products = products.Where(x =>
                        x.Productname.ToLower().Contains(a.ToLower()) ||
                        x.Productdescription.ToLower().Contains(a.ToLower()) ||
                        x.ProductmanufacturerNavigation.Manufacturername.ToLower().Contains(a.ToLower()) ||
                        x.Productcost.ToString().Contains(a.ToLower())).ToList();
                } 
            }
            listProducts.Clear();
            var manufacturerproducts = context.Manufacturerproducts.ToList();
            AssetLoader assetLoader = new AssetLoader();
            foreach (var product in products)
            {
                ListProduct productList = new ListProduct();
                try
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProducts");
                    imagePath = imagePath.Replace("bin\\Debug\\net7.0\\", "");
                    Bitmap image = new Bitmap(Path.Combine(imagePath,product.Productphoto));
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        image.Save(memoryStream);
                    }
                    productList.ImageProduct = image;
                }
                catch (Exception ex)
                {
                    productList.ImageProduct = new Avalonia.Media.Imaging.Bitmap(assetLoader.Open(new Uri("avares://MP4practic/ImageProducts/picture.png")));
                }
                productList.ProductName = product.Productname;
                productList.ManufacturerName = manufacturerproducts.Where(x => product.Productmanufacturer == x.Manufacturerid).First().Manufacturername;
                productList.ProductDescription = product.Productdescription;
                productList.ProductQuantityInStock = product.Productquantityinstock;
                double o = product.Productsall;
                o = o / 100;
                Math.Round(o, 3);
                double p = Decimal.ToDouble(product.Productcost);
                var t = p - (p * o);
                productList.ProductCost = Math.Round(Convert.ToDecimal(t),3);
                productList.Productarticlenumber = product.Productarticlenumber;
                listProducts.Add(productList);
            }

            ListBoxItems.Items = null;
            ListBoxItemsUser.Items = null;
            ListBoxItems.Items = listProducts;
            ListBoxItemsUser.Items = listProducts;
            countItems.Text = listProducts.Count.ToString() + "/" + context.Products.Count().ToString();*/
            LoadProduct();
        }


        private async void AddProduct_OnClick(object? sender, RoutedEventArgs e)
        {
            AddOrRedactProduct add = new AddOrRedactProduct();
            await add.ShowDialog(this);
            UpdateListProduct();
            Proizvoditeli();
        }

        private void Exit_OnClick(object? sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }

        private void ComboUpDown_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            ListBoxItems.Items = null;
            ListBoxItemsUser.Items = null;
            listProducts.Clear();
            LoadProduct();
        }

        public async void InputElement_OnTapped(string sender)
        {
            var product = context.Products.Where(x => x.Productarticlenumber == sender).First();
            if (product != null)
            {
                AddOrRedactProduct addOrRedactProduct = new AddOrRedactProduct(product);
                await addOrRedactProduct.ShowDialog(this);
            }
            UpdateListProduct();
            Proizvoditeli();
        }

    }
}
