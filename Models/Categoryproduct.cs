using System;
using System.Collections.Generic;

namespace MP4practic.Models;

public partial class Categoryproduct
{
    public int Categoryid { get; set; }

    public string Categoryname { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
