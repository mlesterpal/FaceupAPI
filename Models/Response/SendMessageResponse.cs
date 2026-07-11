namespace Faceup.Models.Response;

public class SendMessageResponse
{
    public int MessageId { get; set; }

    public int ConversationId { get; set; }

    public int SenderUserId { get; set; }

    public int RecipientUserId { get; set; }

    public string Body { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
