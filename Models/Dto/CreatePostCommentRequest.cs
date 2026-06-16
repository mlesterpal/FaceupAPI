namespace Faceup.Models.Dto;

public class CreatePostCommentRequest
{
    public int UserId { get; set; }

    public string Comment { get; set; } = string.Empty;
}
