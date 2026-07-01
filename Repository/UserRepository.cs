using Faceup.Models;
using Faceup.Models.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Faceup.Repository;

public class UserRepository
{
    private readonly FaceupContext _context;
    private static readonly ISet<string> SupportedVisibilityFields =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "bio",
            "address",
            "work",
            "highSchool",
            "college",
            "hobbies",
            "phone",
            "gender",
            "birthDate",
        };

    public UserRepository(FaceupContext context)
    {
        _context = context;
    }

    public void AddUser(CreateUser newUser)
    {
        var user = new User
        {
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Password = newUser.Password,
            Email = newUser.Email,
            BirthDate = newUser.BirthDate,
            Gender = newUser.Gender
        };
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? GetUserById(int id)
    {
        return _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
    }

    public User? GetUserByEmail(string email)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Email != null && u.Email.ToLower() == email.ToLower());
    }

    public void UpdateProfilePicture(int userId, string profilePicturePath)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User {userId} not found.");
        }

        user.ProfilePicture = profilePicturePath;
        _context.SaveChanges();
    }

    public void UpdateProfile(int userId, UpdateUserProfileRequest request, ISet<string> presentFields)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User {userId} not found.");
        }

        if (presentFields.Contains("bio"))
        {
            user.Bio = request.Bio;
        }

        if (presentFields.Contains("address"))
        {
            user.Address = request.Address;
        }

        if (presentFields.Contains("work"))
        {
            user.Work = request.Work;
        }

        if (presentFields.Contains("highSchool"))
        {
            user.HighSchool = request.HighSchool;
        }

        if (presentFields.Contains("college"))
        {
            user.College = request.College;
        }

        if (presentFields.Contains("hobbies"))
        {
            user.Hobbies = request.Hobbies;
        }

        if (presentFields.Contains("phone"))
        {
            user.Phone = request.Phone;
        }

        _context.SaveChanges();
    }

    public bool IsSupportedVisibilityField(string fieldName)
    {
        return SupportedVisibilityFields.Contains(fieldName);
    }

    public bool UpdateProfileFieldVisibility(int userId, string fieldName, string visibility)
    {
        var sql = fieldName.ToLowerInvariant() switch
        {
            "bio" => "UPDATE dbo.Users SET BioVisibility = @Visibility WHERE Id = @UserId",
            "address" => "UPDATE dbo.Users SET AddressVisibility = @Visibility WHERE Id = @UserId",
            "work" => "UPDATE dbo.Users SET WorkVisibility = @Visibility WHERE Id = @UserId",
            "highschool" => "UPDATE dbo.Users SET HighSchoolVisibility = @Visibility WHERE Id = @UserId",
            "college" => "UPDATE dbo.Users SET CollegeVisibility = @Visibility WHERE Id = @UserId",
            "hobbies" => "UPDATE dbo.Users SET HobbiesVisibility = @Visibility WHERE Id = @UserId",
            "phone" => "UPDATE dbo.Users SET PhoneVisibility = @Visibility WHERE Id = @UserId",
            "gender" => "UPDATE dbo.Users SET GenderVisibility = @Visibility WHERE Id = @UserId",
            "birthdate" => "UPDATE dbo.Users SET BirthDateVisibility = @Visibility WHERE Id = @UserId",
            _ => null
        };

        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentException("Unsupported profile field.");
        }

        var affectedRows = _context.Database.ExecuteSqlRaw(
            sql,
            new SqlParameter("@Visibility", visibility),
            new SqlParameter("@UserId", userId));

        return affectedRows > 0;
    }
}
