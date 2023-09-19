using System;
using System.Collections.Generic;

namespace MP4practic.Models;

public partial class Unitofmeasurement
{
    public int Unitofmeasurementid { get; set; }

    public string Unitofmeasurement1 { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
