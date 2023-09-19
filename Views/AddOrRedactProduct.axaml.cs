using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Shared.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using Avalonia.Interactivity;
using MessageBox.Avalonia;
using MP4practic.Context;
using MP4practic.Models;
using static System.Net.Mime.MediaTypeNames;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace MP4practic;

public partial class AddOrRedactProduct : Window
{
    private static PostgresContext _context;
    public string ImageName;
    public AddOrRedactProduct()
    {
        DataContext = new DataSourse.AddProductSource();
        InitializeComponent();
        LoadMbAll();
        LoadImage();
    }
    public AddOrRedactProduct(Product product)
    {
        InitializeComponent();
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProducts");
        imagePath = imagePath.Replace("bin\\Debug\\net7.0\\", "");
        if (product.Productphoto == "")
        {
            product.Productphoto = "picture.png";
        }
        Bitmap image = new Bitmap(Path.Combine(imagePath,product.Productphoto));
        using (MemoryStream memoryStream = new MemoryStream())
        {
            image.Save(memoryStream);
        }
        ImageProduct.Source = image;
        LoadMbAll();
        PhotoName.Text = product.Productphoto;
        ImageName = product.Productphoto;
        DataContext = new DataSourse.AddProductSource(product);
        Postavshic.SelectedIndex = product.Productdelivery - 1;
        Proizvod.Text = _context.Manufacturerproducts.Where(X=> X.Manufacturerid == product.Productmanufacturer).First().Manufacturername;
        KategorTovar.SelectedIndex = product.Productcategory - 1;
        EdiProduct.SelectedIndex = product.Productunitofmeasurement - 1;
        Sale.Text = product.Productsall.ToString();
        AddButton.IsVisible = false;
        RedactButton.IsVisible = true;
        Articule.IsEnabled = false;
    }
    public void LoadImage()
    {
        AssetLoader assetLoader = new AssetLoader();
        ImageName = "picture.png";
        var image = new Avalonia.Media.Imaging.Bitmap(assetLoader.Open(new Uri("avares://MP4practic/ImageProducts/picture.png")));
        ImageProduct.Source = image;
    }

    public void LoadMbAll()
    {
        _context = PostgresContext.GetContext();
        Postavshic.Items = _context.Deliveryproducts.ToList();
        KategorTovar.Items = _context.Categoryproducts.ToList();
        EdiProduct.Items = _context.Unitofmeasurements.ToList();
    }

    private void RedactButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Articule.Text != "" || NameProduct.Text != "" || OpisanieProduct.Text != "" || PriceProduct.Text != "" ||
            Sale.Text != "" ||
            MbSale.Text != "" || KolInSclad.Text != "" || KategorTovar.SelectedItem == "" ||
            Postavshic.SelectedItem == "" || Proizvod.Text == "" || EdiProduct.SelectedItem == "")
        {
            if ((int.Parse(Sale.Text) > 100 || int.Parse(Sale.Text) < 0) || (int.Parse(MbSale.Text) > 100 || int.Parse(MbSale.Text) < 0) || (int.Parse(Sale.Text) > int.Parse(MbSale.Text)))
            {
                var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка скидки",
                    "Скидка не может быть больше 100% или меньше 0; действующая скидка не может быть больше максимальной скидки");
                mess.ShowDialog(this);
            }
            else
            {
                var products = _context.Products.ToList();
            var category = _context.Categoryproducts;
            var delivery = _context.Deliveryproducts;
            var manyfac = _context.Manufacturerproducts;
            var unit = _context.Unitofmeasurements;
            Regex regex = new Regex("\\D");
            MatchCollection matchCollection = regex.Matches(Sale.Text);
            MatchCollection matchCollection1 = regex.Matches(MbSale.Text);
            MatchCollection matchCollection2 = regex.Matches(KolInSclad.Text);
            MatchCollection matchCollection3 = regex.Matches(PriceProduct.Text.Split(',')[0]);
            if (matchCollection.Count != 0 || matchCollection1.Count != 0 || matchCollection2.Count != 0 ||
                matchCollection3.Count != 0)
            {
                var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка данных",
                    "Вы ввели не числовые символы в числовые поля или ввели минусовые значения данных");
                mess.ShowDialog(this);
                Sale.Text = "0";
                MbSale.Text = "0";
                KolInSclad.Text = "0";
                PriceProduct.Text = "0";
                
            }
            else
            {
                foreach (var VARIABLE in products)
                {
                    try
                    {
                        if (VARIABLE.Productarticlenumber == Articule.Text)
                        {
                            VARIABLE.Productcategory = category
                                .Where(x => x.Categoryid == KategorTovar.SelectedIndex + 1).First().Categoryid;
                            VARIABLE.Productdelivery = delivery
                                .Where(x => x.Deliveryid == Postavshic.SelectedIndex + 1)
                                .First().Deliveryid;
                            VARIABLE.Productphoto = PhotoName.Text;
                            if (decimal.Parse(PriceProduct.Text) < 0)
                            {
                                VARIABLE.Productcost = 0;
                            }
                            else
                            {
                                VARIABLE.Productcost = decimal.Parse(PriceProduct.Text);
                            }

                            VARIABLE.Productdescription = OpisanieProduct.Text;
                            if (int.Parse(MbSale.Text) < 0)
                            {
                                VARIABLE.Productdiscountamount = 0;
                            }
                            else
                            {
                                VARIABLE.Productdiscountamount = short.Parse(MbSale.Text);
                            }

                            Manufacturerproduct? man = new Manufacturerproduct();
                            try
                            {
                                man = manyfac.Where(x => x.Manufacturername == Proizvod.Text).First();
                            }
                            catch (Exception exception)
                            {
                                man = null;
                            }
                            if (man == null)
                            {
                                Manufacturerproduct ma = new Manufacturerproduct();
                                ma.Manufacturername = Proizvod.Text;
                                manyfac.Add(ma);
                                _context.SaveChanges();
                            }
                            VARIABLE.Productmanufacturer = manyfac
                                .Where(x => x.Manufacturername == Proizvod.Text).First().Manufacturerid;
                            VARIABLE.Productname = NameProduct.Text;
                            if (int.Parse(Sale.Text) < 0)
                            {
                                VARIABLE.Productsall = 0;
                            }
                            else
                            {
                                VARIABLE.Productsall = int.Parse(Sale.Text);
                            }

                            if (int.Parse(KolInSclad.Text) < 0)
                            {
                                VARIABLE.Productquantityinstock = 0;

                            }
                            else
                            {
                                VARIABLE.Productquantityinstock = int.Parse(KolInSclad.Text);
                            }

                            VARIABLE.Productunitofmeasurement = unit
                                .Where(x => x.Unitofmeasurementid == EdiProduct.SelectedIndex + 1).First()
                                .Unitofmeasurementid;
                            _context.SaveChanges();
                            var mess = MessageBoxManager.GetMessageBoxStandardWindow("Редактирование товара",
                                "Продукт отредактирован");
                            mess.ShowDialog(this);
                        }
                    }
                    catch (Exception exception)
                    {
                        var mess = MessageBoxManager.GetMessageBoxStandardWindow("Редактирование товара",
                            "Продукт не отредактирован" + "/n" + exception.ToString());
                        mess.ShowDialog(this);
                    }
                }
            }
            }
        }
        else
        {
            var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка сохранения", "Вы не заполнили все поля");
            mess.ShowDialog(this);
        }
    }

    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var manyfac = _context.Manufacturerproducts;
        try
        {
            if (Articule.Text != "" || NameProduct.Text != "" || OpisanieProduct.Text != "" ||
                PriceProduct.Text != "" ||
                Sale.Text != "" ||
                MbSale.Text != "" || KolInSclad.Text != "" || KategorTovar.SelectedItem.ToString() == "" ||
                Postavshic.SelectedItem.ToString() == "" || Proizvod.Text == "" ||
                EdiProduct.SelectedItem.ToString() == "")
            {
                if ((int.Parse(Sale.Text) > 100 || int.Parse(Sale.Text) < 0) ||
                    (int.Parse(MbSale.Text) > 100 || int.Parse(MbSale.Text) < 0) ||
                    (int.Parse(Sale.Text) > int.Parse(MbSale.Text)))
                {
                    var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка скидки",
                        "Скидка не может быть больше 100% или меньше 0; действующая скидка не может быть больше максимальной скидки");
                    mess.ShowDialog(this);
                }
                else
                {
                    Regex regex = new Regex("\\D");
                    MatchCollection matchCollection = regex.Matches(Sale.Text);
                    MatchCollection matchCollection1 = regex.Matches(MbSale.Text);
                    MatchCollection matchCollection2 = regex.Matches(KolInSclad.Text);
                    MatchCollection matchCollection3 = regex.Matches(PriceProduct.Text.Split(',')[0]);
                    if (matchCollection.Count != 0 || matchCollection1.Count != 0 || matchCollection2.Count != 0 ||
                        matchCollection3.Count != 0)
                    {
                        var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка данных",
                            "Вы ввели не числовые символы в числовые поля или ввели минусовые значения данных");
                        mess.ShowDialog(this);
                        Sale.Text = "0";
                        MbSale.Text = "0";
                        KolInSclad.Text = "0";
                        PriceProduct.Text = "0";

                    }
                    else
                    {
                        var product = new Product();
                        product.Productcategory = _context.Categoryproducts
                            .Where(x => x.Categoryid == KategorTovar.SelectedIndex + 1).First().Categoryid;
                        product.Productdelivery = _context.Deliveryproducts
                            .Where(x => x.Deliveryid == Postavshic.SelectedIndex + 1)
                            .First().Deliveryid;
                        product.Productphoto = PhotoName.Text;
                        product.Productcost = decimal.Parse(PriceProduct.Text);
                        product.Productdescription = OpisanieProduct.Text;
                        product.Productdiscountamount = short.Parse(Sale.Text);
                        Manufacturerproduct? man = new Manufacturerproduct();
                        try
                        {
                            man = manyfac.Where(x => x.Manufacturername == Proizvod.Text).First();
                        }
                        catch (Exception exception)
                        {
                            man = null;
                        }
                        if (man == null)
                        {
                            Manufacturerproduct ma = new Manufacturerproduct();
                            ma.Manufacturername = Proizvod.Text;
                            manyfac.Add(ma);
                            _context.SaveChanges();
                        }

                        product.Productmanufacturer = _context.Manufacturerproducts
                            .Where(x => x.Manufacturername == Proizvod.Text).First().Manufacturerid;
                        product.Productname = NameProduct.Text;
                        product.Productquantityinstock = int.Parse(KolInSclad.Text);
                        product.Productunitofmeasurement = _context.Unitofmeasurements
                            .Where(x => x.Unitofmeasurementid == EdiProduct.SelectedIndex + 1).First()
                            .Unitofmeasurementid;
                        product.Productarticlenumber = Articule.Text;
                        try
                        {
                            _context.Products.Add(product);
                            _context.SaveChanges();
                            var mess = MessageBoxManager.GetMessageBoxStandardWindow("Добавление товара",
                                "Продукт добавлен");
                            mess.ShowDialog(this);
                        }
                        catch (Exception exception)
                        {
                            var mess = MessageBoxManager.GetMessageBoxStandardWindow("Добавление товара",
                                "Продукт не добавлен" + "\n" + exception.ToString());
                            mess.ShowDialog(this);
                        }
                    }
                    
                }
                
            }
            else
            {
                var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка сохранения",
                    "Вы не заполнили все поля");
                mess.ShowDialog(this);
            }
        }
        catch (Exception exception)
        {
            var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка сохранения",
                "Вы не заполнили все поля");
            mess.ShowDialog(this);
        }

    }

    private async void AddImage_OnClick(object? sender, RoutedEventArgs e)
    {
        
        OpenFileDialog save = new OpenFileDialog();
        save.Title = "Добавление картинки";
        save.AllowMultiple = true;
        save.Filters = new List<FileDialogFilter>()
        {
            new FileDialogFilter()
            {
                Name = "Картинки", Extensions = new List<string>() { "img","png","jpg"}
            }
        };
        var str = await save.ShowAsync(this);
        var result = str[0];
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProducts");
        imagePath = imagePath.Replace("bin\\Debug\\net7.0\\", "");
        string fileName = Path.GetFileName(result);
        string comboFileName = Path.Combine(imagePath, fileName);
        try
        {
            if (result == comboFileName)
            {
            
                PhotoName.Text = fileName;
                Bitmap bitmap = new Bitmap(comboFileName);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream);
                    
                    ImageProduct.Source = bitmap;
                }
            }
            else if (result != comboFileName)
            {
                File.Copy(result,comboFileName,true);
                PhotoName.Text = fileName;
                Bitmap bitmap = new Bitmap(comboFileName);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream);
                    ImageProduct.Source = bitmap;
                }
            }
        }
        catch (Exception exception)
        {
            var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка загрузки", "Ошибка загрузки картинки");
            mess.ShowDialog(this);
        }
        
        
        
        
    }
}