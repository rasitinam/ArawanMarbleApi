using System;
using System.Collections.Generic;

namespace ArawanMarbleApi.Models;

public partial class Product
{
    public int Productid { get; set; }

    public string Productname { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Productimg { get; set; } = null!;

    public virtual ICollection<SubProduct> SubProducts { get; set; } = new List<SubProduct>();
}
