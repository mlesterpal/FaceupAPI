namespace Faceup.Models.Response;

public class PostLikeUserResponse
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ProfilePicture { get; set; }

    public DateTime LikedAt { get; set; }
}
