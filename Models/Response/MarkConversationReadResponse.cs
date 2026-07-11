namespace Faceup.Models.Response;

public class MarkConversationReadResponse
{
    public int ConversationId { get; set; }

    public int UserId { get; set; }

    public int MarkedCount { get; set; }
}
