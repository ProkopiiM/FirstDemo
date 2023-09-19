using System;
using System.Collections.Generic;

namespace MP4practic.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public DateTime Orderdate { get; set; }

    public DateTime Orderdeliverydate { get; set; }

    public int Orderpickuppoint { get; set; }

    public int Ordercodetoget { get; set; }

    public int? Orderuserid { get; set; }

    public int? Orderstatus { get; set; }

    public virtual Pickuppoint OrderpickuppointNavigation { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual User? Orderuser { get; set; }
}
