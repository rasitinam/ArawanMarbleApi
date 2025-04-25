using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArawanMarbleApi.Models;

public partial class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //
    public int Productid { get; set; }

    public string Productname { get; set; } = null!;

    public string? Description { get; set; }

    public string? Productimg { get; set; }
}
