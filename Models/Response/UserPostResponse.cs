namespace Faceup.Models.Response;

public class UserPostResponse
{
    public int PostId { get; set; }

    public string? Message { get; set; }

    public string? ImageUrl { get; set; }

    public string? FirstName { get; set; }

    public string? ProfilePicture { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int LikeCount { get; set; }

    public bool IsLiked { get; set; }

    public int ShareCount { get; set; }

    public bool IsShared { get; set; }

    public int CommentCount { get; set; }
}
