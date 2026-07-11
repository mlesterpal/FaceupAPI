namespace Faceup.Models.Response;

public class ConversationMessageResponse
{
    public int MessageId { get; set; }

    public int ConversationId { get; set; }

    public int SenderUserId { get; set; }

    public string? SenderFirstName { get; set; }

    public string? SenderLastName { get; set; }

    public string? SenderProfilePicture { get; set; }

    public string Body { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsRead { get; set; }
}
