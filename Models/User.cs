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

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Friendship> FriendshipReceivers { get; set; } = new List<Friendship>();

    public virtual ICollection<Friendship> FriendshipRequesters { get; set; } = new List<Friendship>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();
}
