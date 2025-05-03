using System;
using System.Collections.Generic;

namespace ArawanMarbleApi.Models;

public partial class SubProduct
{
    public int SubProductid { get; set; }

    public int Productid { get; set; }

    public string Productname { get; set; } = null!;

    public string Productimg { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
