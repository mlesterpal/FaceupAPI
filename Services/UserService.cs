using Faceup.Models.Dto;
using Faceup.Models.Response;
using Faceup.Repository;

namespace Faceup.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly FileStorageService _fileStorage;
    private readonly TokenService _tokenService;

    public UserService(UserRepository userRepository, FileStorageService fileStorage, TokenService tokenService)
    {
        _userRepository = userRepository;
        _fileStorage = fileStorage;
        _tokenService = tokenService;
    }

    public LoginUserResponse AddUser(CreateUser newUser)
    {
        if (string.IsNullOrWhiteSpace(newUser.FirstName) ||
            string.IsNullOrWhiteSpace(newUser.LastName) ||
            string.IsNullOrWhiteSpace(newUser.Email) ||
            string.IsNullOrWhiteSpace(newUser.Password) ||
            string.IsNullOrWhiteSpace(newUser.Gender))
        {
            throw new ArgumentException("FirstName, LastName, Email, Password, and Gender are required.");
        }

        if (_userRepository.GetUserByEmail(newUser.Email) != null)
        {
            throw new InvalidOperationException("This email is already registered.");
        }

        var createdUser = _userRepository.AddUser(newUser);
        return BuildLoginResponse(createdUser);
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
            Phone = user.Phone,
            BioVisibility = user.BioVisibility,
            AddressVisibility = user.AddressVisibility,
            WorkVisibility = user.WorkVisibility,
            HighSchoolVisibility = user.HighSchoolVisibility,
            CollegeVisibility = user.CollegeVisibility,
            HobbiesVisibility = user.HobbiesVisibility,
            PhoneVisibility = user.PhoneVisibility,
            GenderVisibility = user.GenderVisibility,
            BirthDateVisibility = user.BirthDateVisibility
        };
    }

    public List<UserListItemResponse> GetUsers()
    {
        return _userRepository.GetUsers();
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

    public UserProfileResponse UpdateUserProfile(
        int userId,
        UpdateUserProfileRequest request,
        ISet<string> presentFields)
    {
        var user = _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User {userId} not found.");
        }

        _userRepository.UpdateProfile(userId, request, presentFields);

        return GetUserProfile(userId)
            ?? throw new KeyNotFoundException($"User {userId} not found.");
    }

    public UpdateProfileFieldVisibilityResponse UpdateProfileFieldVisibility(
        int userId,
        string fieldName,
        string visibility)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("UserId must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(fieldName))
        {
            throw new ArgumentException("FieldName is required.");
        }

        if (string.IsNullOrWhiteSpace(visibility))
        {
            throw new ArgumentException("Visibility is required.");
        }

        var normalizedFieldName = fieldName.Trim();
        var normalizedVisibility = visibility.Trim();

        if (!string.Equals(normalizedVisibility, "Public", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(normalizedVisibility, "Private", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(normalizedVisibility, "Pinned", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Visibility must be Public, Private, or Pinned.");
        }

        if (!_userRepository.IsSupportedVisibilityField(normalizedFieldName))
        {
            throw new ArgumentException("Unsupported profile field.");
        }

        var normalizedVisibilityValue = char.ToUpper(normalizedVisibility[0]) + normalizedVisibility[1..].ToLower();
        var isUpdated = _userRepository.UpdateProfileFieldVisibility(
            userId,
            normalizedFieldName,
            normalizedVisibilityValue);

        if (!isUpdated)
        {
            throw new KeyNotFoundException($"User {userId} not found.");
        }

        return new UpdateProfileFieldVisibilityResponse
        {
            UserId = userId,
            FieldName = normalizedFieldName,
            Visibility = normalizedVisibilityValue,
            Message = "Profile field visibility updated successfully."
        };
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

        return await Task.FromResult(BuildLoginResponse(user));
    }

    public UserProfileResponse GetCurrentUserProfile(int userId)
    {
        return GetUserProfile(userId)
            ?? throw new KeyNotFoundException("User not found.");
    }

    private LoginUserResponse BuildLoginResponse(Models.User user)
    {
        return new LoginUserResponse
        {
            Token = _tokenService.GenerateToken(user),
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture
        };
    }
}
