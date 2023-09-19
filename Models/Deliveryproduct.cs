using System;
using System.Collections.Generic;

namespace MP4practic.Models;

public partial class Deliveryproduct
{
    public int Deliveryid { get; set; }

    public string Deliveryname { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
