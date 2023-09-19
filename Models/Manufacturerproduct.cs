using System;
using System.Collections.Generic;

namespace MP4practic.Models;

public partial class Manufacturerproduct
{
    public int Manufacturerid { get; set; }

    public string Manufacturername { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
