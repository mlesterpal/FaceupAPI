using System;
using System.Collections.Generic;

namespace Faceup.Models;

public partial class Post
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public int UserId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual User User { get; set; } = null!;
}
