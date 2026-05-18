using Microsoft.Extensions.Options;

namespace Faceup.Services;

public class FileUploadOptions
{
    public const string SectionName = "FileUpload";

    public long MaxFileSizeBytes { get; set; } = 5_242_880;

    public string[] AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
}

public class FileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly FileUploadOptions _options;

    public FileStorageService(IWebHostEnvironment environment, IOptions<FileUploadOptions> options)
    {
        _environment = environment;
        _options = options.Value;
    }

    public async Task<string?> SaveImageAsync(IFormFile? image, CancellationToken cancellationToken = default)
    {
        if (!CheckImageNullOrLength(image, required: false))
        {
            return null;
        }

        var extension = CheckIfImageExtensionAllowed(image!, _options.AllowedExtensions);
        CheckIfImageExceedsMaximumSize(image!, _options.MaxFileSizeBytes);

        var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
        EnsureDirectoryExists(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await SaveFileToDiskAsync(image!, filePath, cancellationToken);

        return ToWebRelativePath("uploads", fileName);
    }

    public async Task<string> SaveProfilePictureAsync(
        int userId,
        IFormFile image,
        string? existingRelativePath = null,
        CancellationToken cancellationToken = default)
    {
        CheckImageNullOrLength(image, required: true);

        var extension = CheckIfImageExtensionAllowed(image, _options.AllowedExtensions);
        CheckIfImageExceedsMaximumSize(image, _options.MaxFileSizeBytes);

        var profilesPath = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
        EnsureDirectoryExists(profilesPath);

        DeleteExistingFileIfPresent(existingRelativePath);

        var fileName = $"user-{userId}{extension}";
        var filePath = Path.Combine(profilesPath, fileName);

        await SaveFileToDiskAsync(image, filePath, cancellationToken);

        return ToWebRelativePath("uploads", "profiles", fileName);
    }

    private static bool CheckImageNullOrLength(IFormFile? image, bool required)
    {
        if (image == null || image.Length == 0)
        {
            if (required)
            {
                throw new InvalidOperationException("Image file is required.");
            }

            return false;
        }

        return true;
    }

    private static string CheckIfImageExtensionAllowed(IFormFile image, string[] allowedExtensions)
    {
        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException(
                $"File type not allowed. Allowed: {string.Join(", ", allowedExtensions)}");
        }

        return extension;
    }

    private static void CheckIfImageExceedsMaximumSize(IFormFile image, long maxFileSizeBytes)
    {
        if (image.Length > maxFileSizeBytes)
        {
            throw new InvalidOperationException(
                $"File exceeds maximum size of {maxFileSizeBytes / 1_048_576} MB.");
        }
    }

    private static void EnsureDirectoryExists(string absoluteDirectoryPath)
    {
        Directory.CreateDirectory(absoluteDirectoryPath);
    }

    private static async Task SaveFileToDiskAsync(
        IFormFile image,
        string absoluteFilePath,
        CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(absoluteFilePath, FileMode.Create);
        await image.CopyToAsync(stream, cancellationToken);
    }

    private void DeleteExistingFileIfPresent(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            return;
        }

        var existingFile = Path.Combine(
            _environment.WebRootPath,
            relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(existingFile))
        {
            File.Delete(existingFile);
        }
    }

    private static string ToWebRelativePath(params string[] segments)
        => "/" + string.Join("/", segments);
}
