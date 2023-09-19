using Avalonia.Shared.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP4practic.ModelsDB
{
    public class ListProduct
    {
        public string Productarticlenumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ManufacturerName { get; set; }
        public int ProductQuantityInStock { get; set; }
        public decimal ProductCost { get; set; }
        public int ProductManufacturer { get; set; }
        public Avalonia.Media.Imaging.Bitmap ImageProduct { get; set; }

    }
    
}
