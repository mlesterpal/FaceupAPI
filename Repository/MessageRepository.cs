using Faceup.Models;
using Faceup.Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Faceup.Repository;

public class MessageRepository
{
    private readonly FaceupContext _context;

    public MessageRepository(FaceupContext context)
    {
        _context = context;
    }

    public bool UserExists(int userId)
    {
        return _context.Users.Any(u => u.Id == userId);
    }

    public bool IsConversationParticipant(int conversationId, int userId)
    {
        return _context.Conversations
            .AsNoTracking()
            .Any(c => c.Id == conversationId && (c.User1Id == userId || c.User2Id == userId));
    }

    public int? GetCounterpartUserId(int conversationId, int userId)
    {
        return _context.Conversations
            .AsNoTracking()
            .Where(c => c.Id == conversationId && (c.User1Id == userId || c.User2Id == userId))
            .Select(c => c.User1Id == userId ? c.User2Id : c.User1Id)
            .FirstOrDefault();
    }

    public bool AreAcceptedFriends(int userId, int otherUserId)
    {
        return _context.Friendships
            .AsNoTracking()
            .Any(f =>
                f.Status == "Accepted" &&
                (
                    (f.RequesterId == userId && f.ReceiverId == otherUserId) ||
                    (f.RequesterId == otherUserId && f.ReceiverId == userId)
                ));
    }

    public List<ConversationListItemResponse> GetConversations(int userId)
    {
        return _context.Database
            .SqlQueryRaw<ConversationListItemResponse>(
                "EXEC dbo.usp_GetUserConversations @UserId",
                new SqlParameter("@UserId", userId))
            .ToList();
    }

    public List<ConversationMessageResponse> GetConversationMessages(int conversationId, int userId)
    {
        return _context.Database
            .SqlQueryRaw<ConversationMessageResponse>(
                "EXEC dbo.usp_GetConversationMessages @ConversationId, @UserId",
                new SqlParameter("@ConversationId", conversationId),
                new SqlParameter("@UserId", userId))
            .ToList();
    }

    public SendMessageResponse SendMessage(
        int userId,
        int? conversationId,
        int? recipientUserId,
        string body)
    {
        var results = _context.Database
            .SqlQueryRaw<SendMessageResponse>(
                "EXEC dbo.usp_SendMessage @UserId, @ConversationId, @RecipientUserId, @Body",
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ConversationId", (object?)conversationId ?? DBNull.Value),
                new SqlParameter("@RecipientUserId", (object?)recipientUserId ?? DBNull.Value),
                new SqlParameter("@Body", body))
            .ToList();

        var response = results.FirstOrDefault();
        if (response == null)
        {
            throw new InvalidOperationException("Message was not created.");
        }

        return response;
    }

    public int MarkConversationRead(int conversationId, int userId)
    {
        var result = _context.Database
            .SqlQueryRaw<MarkedCountResult>(
                "EXEC dbo.usp_MarkConversationRead @ConversationId, @UserId",
                new SqlParameter("@ConversationId", conversationId),
                new SqlParameter("@UserId", userId))
            .AsEnumerable()
            .FirstOrDefault();

        return result?.MarkedCount ?? 0;
    }

    public int GetUnreadConversationCount(int userId)
    {
        var result = _context.Database
            .SqlQueryRaw<UnreadConversationCountResponse>(
                "EXEC dbo.usp_GetUnreadConversationCount @UserId",
                new SqlParameter("@UserId", userId))
            .AsEnumerable()
            .FirstOrDefault();

        return result?.UnreadConversationCount ?? 0;
    }

    private class MarkedCountResult
    {
        public int MarkedCount { get; set; }
    }
}
