namespace Faceup.Models.Response;

public class DeleteUserPostResponse
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public bool Deleted { get; set; }
    public string Message { get; set; } = string.Empty;
}
