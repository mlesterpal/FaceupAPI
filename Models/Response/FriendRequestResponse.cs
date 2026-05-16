namespace Faceup.Models.Response;

public class FriendRequestResponse
{
    public int FriendshipId { get; set; }

    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; }
}
