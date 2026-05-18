using System;
using System.Collections.Generic;

namespace Faceup.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Password { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? ProfilePicture { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
