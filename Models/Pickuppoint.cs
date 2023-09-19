using System;
using System.Collections.Generic;

namespace MP4practic.Models;

public partial class Pickuppoint
{
    public int Pickuppointid { get; set; }

    public int Pickuppointercode { get; set; }

    public string Pickuppointeraddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
