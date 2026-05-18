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
}
