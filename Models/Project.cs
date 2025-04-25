using System;
using System.Collections.Generic;

namespace ArawanMarbleApi.Models;

public partial class Project
{
    public int Projectid { get; set; }

    public string Projectname { get; set; } = null!;

    public string? Projectimg { get; set; }

    public string? Projectplace { get; set; }

    public string? Description { get; set; }
}
