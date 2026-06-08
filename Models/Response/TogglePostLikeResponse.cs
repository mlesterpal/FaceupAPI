namespace Faceup.Models.Response;

public class TogglePostLikeResponse
{
    public int PostId { get; set; }

    public int UserId { get; set; }

    public bool Liked { get; set; }

    public int LikeCount { get; set; }

    public string Message { get; set; } = string.Empty;
}
