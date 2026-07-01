namespace Faceup.Models.Dto;

public class UpdateProfileFieldVisibilityRequest
{
    public string FieldName { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
}
