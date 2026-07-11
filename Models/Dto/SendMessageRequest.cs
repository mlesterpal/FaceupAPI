namespace Faceup.Models.Dto;

public class SendMessageRequest
{
    public int? ConversationId { get; set; }

    public int? RecipientUserId { get; set; }

    public string Body { get; set; } = string.Empty;
}
