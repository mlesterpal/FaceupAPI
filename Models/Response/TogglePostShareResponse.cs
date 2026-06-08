namespace Faceup.Models.Response;

public class TogglePostShareResponse
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public bool IsShared { get; set; }
    public int ShareCount { get; set; }
    public string Message { get; set; } = string.Empty;
}