namespace Faceup.Models.Response;

public class UserProfileResponse
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Bio { get; set; }

    public string? Address { get; set; }

    public string? Work { get; set; }

    public string? HighSchool { get; set; }

    public string? College { get; set; }

    public string? Hobbies { get; set; }

    public string? Phone { get; set; }
}
