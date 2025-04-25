using System;
using System.Collections.Generic;

namespace ArawanMarbleApi.Models;

public partial class Customer
{
    public int Customerid { get; set; }

    public string Customername { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Customeremail { get; set; }

    public string? Customersubject { get; set; }

    public string? Customermessage { get; set; }
}
