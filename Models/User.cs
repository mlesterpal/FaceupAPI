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

    public string? Bio { get; set; }

    public string? Address { get; set; }

    public string? Work { get; set; }

    public string? HighSchool { get; set; }

    public string? College { get; set; }

    public string? Hobbies { get; set; }

    public string? Phone { get; set; }

    public string? BioVisibility { get; set; }

    public string? AddressVisibility { get; set; }

    public string? WorkVisibility { get; set; }

    public string? HighSchoolVisibility { get; set; }

    public string? CollegeVisibility { get; set; }

    public string? HobbiesVisibility { get; set; }

    public string? PhoneVisibility { get; set; }

    public string? GenderVisibility { get; set; }

    public string? BirthDateVisibility { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Conversation> ConversationUser1s { get; set; } = new List<Conversation>();

    public virtual ICollection<Conversation> ConversationUser2s { get; set; } = new List<Conversation>();

    public virtual ICollection<Friendship> FriendshipReceivers { get; set; } = new List<Friendship>();

    public virtual ICollection<Friendship> FriendshipRequesters { get; set; } = new List<Friendship>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();
}
