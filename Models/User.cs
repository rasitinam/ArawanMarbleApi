using System;
using System.Collections.Generic;

namespace ArawanMarbleApi.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
