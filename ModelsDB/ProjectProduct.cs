using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP4practic.ModelsDB
{
    public class ProjectProduct
    {
        public string ProductArticleNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductCategory { get; set; }
        public string ProductPhoto { get; set; }
        public int ProductManufacturer { get; set; }
        public int ProductDelivery {  get; set; }
        public decimal ProductCost { get; set; }
        public int ProductDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public int ProductUnitOfMeasurement { get; set; }

    }
}
