using Faceup.Models;
using Faceup.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Faceup.Repository;

public class UserRepository
{
    private readonly FaceupContext _context;

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

    public void UpdateProfile(int userId, UpdateUserProfileRequest request)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User {userId} not found.");
        }

        user.Bio = request.Bio;
        user.Address = request.Address;
        user.Work = request.Work;
        user.HighSchool = request.HighSchool;
        user.College = request.College;
        user.Hobbies = request.Hobbies;
        user.Phone = request.Phone;

        _context.SaveChanges();
    }
}
