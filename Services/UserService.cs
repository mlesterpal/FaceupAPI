using Faceup.Models.Dto;
using Faceup.Models.Response;
using Faceup.Repository;

namespace Faceup.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly FileStorageService _fileStorage;

    public UserService(UserRepository userRepository, FileStorageService fileStorage)
    {
        _userRepository = userRepository;
        _fileStorage = fileStorage;
    }

    public void AddUser(CreateUser newUser)
    {
        _userRepository.AddUser(newUser);
    }

    public UserProfileResponse? GetUserProfile(int userId)
    {
        var user = _userRepository.GetUserById(userId);
        if (user == null)
        {
            return null;
        }

        return new UserProfileResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture
        };
    }

    public async Task<string> UpdateProfilePictureAsync(
        int userId,
        IFormFile image,
        CancellationToken cancellationToken = default)
    {
        var user = _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User {userId} not found.");
        }

        var profilePictureUrl = await _fileStorage.SaveProfilePictureAsync(
            userId,
            image,
            user.ProfilePicture,
            cancellationToken);

        _userRepository.UpdateProfilePicture(userId, profilePictureUrl);

        return profilePictureUrl;
    }
}
