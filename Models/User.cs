using System;
using System.Collections.Generic;

namespace Faceup.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }
}
