namespace Faceup.Models.Response;

public class PostCommentUserResponse
{
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Content { get; set; }
    public DateTime CommentedAt { get; set; }
}