namespace Faceup.Models.Response;

public class FriendUserResponse
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? FriendshipId { get; set; }

    public DateTime? FriendsSince { get; set; }
}
