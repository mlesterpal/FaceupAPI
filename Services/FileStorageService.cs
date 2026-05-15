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
        if (image == null || image.Length == 0)
        {
            return null;
        }

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!_options.AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException(
                $"File type not allowed. Allowed: {string.Join(", ", _options.AllowedExtensions)}");
        }

        if (image.Length > _options.MaxFileSizeBytes)
        {
            throw new InvalidOperationException(
                $"File exceeds maximum size of {_options.MaxFileSizeBytes / 1_048_576} MB.");
        }

        var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream, cancellationToken);

        return $"/uploads/{fileName}";
    }
}
