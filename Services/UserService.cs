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
            ProfilePicture = user.ProfilePicture,
            Email = user.Email,
            Gender = user.Gender,
            BirthDate = user.BirthDate,
            Bio = user.Bio,
            Address = user.Address,
            Work = user.Work,
            HighSchool = user.HighSchool,
            College = user.College,
            Hobbies = user.Hobbies,
            Phone = user.Phone
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

    public UserProfileResponse UpdateUserProfile(int userId, UpdateUserProfileRequest request)
    {
        var user = _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User {userId} not found.");
        }

        _userRepository.UpdateProfile(userId, request);

        return GetUserProfile(userId)
            ?? throw new KeyNotFoundException($"User {userId} not found.");
    }

    public async Task<LoginUserResponse> LoginAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken = default)
    {
        var user = _userRepository.GetUserByEmail(loginUserRequest.Email);
        if (user == null)
        {
            throw new KeyNotFoundException($"User {loginUserRequest.Email} not found.");
        }

        if (!string.Equals(user.Password, loginUserRequest.Password, StringComparison.Ordinal))
        {
            throw new UnauthorizedAccessException("Invalid password.");
        }

        return await Task.FromResult(new LoginUserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture
        });
    }
}
