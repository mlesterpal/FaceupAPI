using Microsoft.AspNetCore.Http;

namespace Faceup.Models.Dto;

public class CreatePostRequest
{
    public string? Message { get; set; }

    public IFormFile? Image { get; set; }
}
