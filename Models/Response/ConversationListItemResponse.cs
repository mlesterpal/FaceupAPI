namespace Faceup.Models.Response;

public class ConversationListItemResponse
{
    public int ConversationId { get; set; }

    public int OtherUserId { get; set; }

    public string? OtherUserFirstName { get; set; }

    public string? OtherUserLastName { get; set; }

    public string? OtherUserProfilePicture { get; set; }

    public int? LastMessageId { get; set; }

    public int? LastMessageSenderUserId { get; set; }

    public string? LastMessageBody { get; set; }

    public DateTime? LastMessageCreatedAt { get; set; }

    public DateTime? LastMessageAt { get; set; }

    public int UnreadCount { get; set; }
}
