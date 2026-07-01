namespace Faceup.Models.Response;

public class UpdateProfileFieldVisibilityResponse
{
    public int UserId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
