namespace Faceup.Models.Dto;

public class CreateNotificationRequest
{
    public int RecipientUserId { get; set; }
    public int? ActorUserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? RelatedEntityType { get; set; }

    // Forward-compatible input for future schema expansion.
    public int? RelatedEntityId { get; set; }
}
