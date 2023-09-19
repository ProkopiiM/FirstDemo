using System;
using System.Collections.Generic;

namespace MP4practic.Models;

public partial class Product
{
    public string Productarticlenumber { get; set; } = null!;

    public string Productname { get; set; } = null!;

    public string Productdescription { get; set; } = null!;

    public int Productcategory { get; set; }

    public string? Productphoto { get; set; }

    public int Productmanufacturer { get; set; }

    public int Productdelivery { get; set; }

    public decimal Productcost { get; set; }

    public short Productdiscountamount { get; set; }

    public int Productquantityinstock { get; set; }

    public int Productunitofmeasurement { get; set; }

    public int Productsall { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Categoryproduct ProductcategoryNavigation { get; set; } = null!;

    public virtual Deliveryproduct ProductdeliveryNavigation { get; set; } = null!;

    public virtual Manufacturerproduct ProductmanufacturerNavigation { get; set; } = null!;

    public virtual Unitofmeasurement ProductunitofmeasurementNavigation { get; set; } = null!;
}
