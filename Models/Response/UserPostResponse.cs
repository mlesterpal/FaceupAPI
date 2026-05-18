namespace Faceup.Models.Response;

public class UserPostResponse
{
    public int PostId { get; set; }

    public string? Message { get; set; }

    public string? ImageUrl { get; set; }

    public string? FirstName { get; set; }

    public string? ProfilePicture { get; set; }

    public DateTime CreatedAt { get; set; }
}
