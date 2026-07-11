namespace Faceup.Models.Response;

public class UserListItemResponse
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; }

    public string? ProfilePicture { get; set; }
}
