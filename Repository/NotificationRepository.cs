using Faceup.Models;
using Faceup.Models.Dto;
using Faceup.Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Faceup.Repository;

public class NotificationRepository
{
    private readonly FaceupContext _context;

    public NotificationRepository(FaceupContext context)
    {
        _context = context;
    }

    public void CreateNotification(CreateNotificationRequest request, string message)
    {
        if (!_context.Users.Any(u => u.Id == request.RecipientUserId))
        {
            throw new KeyNotFoundException("Recipient user not found.");
        }

        if (request.ActorUserId.HasValue && !_context.Users.Any(u => u.Id == request.ActorUserId.Value))
        {
            throw new KeyNotFoundException("Actor user not found.");
        }

        _context.Database.ExecuteSqlRaw(
            @"INSERT INTO dbo.Notifications (RecipientUserId, ActorUserId, Type, Message, RelatedEntityType, RelatedEntityId)
              VALUES (@RecipientUserId, @ActorUserId, @Type, @Message, @RelatedEntityType, @RelatedEntityId)",
            new SqlParameter("@RecipientUserId", request.RecipientUserId),
            new SqlParameter("@ActorUserId", (object?)request.ActorUserId ?? DBNull.Value),
            new SqlParameter("@Type", request.Type),
            new SqlParameter("@Message", message),
            new SqlParameter("@RelatedEntityType", (object?)request.RelatedEntityType ?? DBNull.Value),
            new SqlParameter("@RelatedEntityId", (object?)request.RelatedEntityId ?? DBNull.Value));
    }

    public string GetActorDisplayName(int? actorUserId)
    {
        if (!actorUserId.HasValue)
        {
            return "Someone";
        }

        var user = _context.Users
            .AsNoTracking()
            .Where(u => u.Id == actorUserId.Value)
            .Select(u => new { u.FirstName, u.LastName, u.Email })
            .FirstOrDefault();

        if (user == null)
        {
            throw new KeyNotFoundException("Actor user not found.");
        }

        var fullName = $"{user.FirstName ?? string.Empty} {user.LastName ?? string.Empty}".Trim();
        if (!string.IsNullOrWhiteSpace(fullName))
        {
            return fullName;
        }

        return string.IsNullOrWhiteSpace(user.Email) ? "Someone" : user.Email;
    }

    public List<UserNotificationResponse> GetNotificationsByRecipientUserId(int userId)
    {
        var userIdParam = new SqlParameter("@UserId", userId);

        return _context.Database
            .SqlQueryRaw<UserNotificationResponse>(
                "EXEC dbo.usp_GetUserNotifications @UserId",
                userIdParam)
            .ToList();
    }

    public bool MarkAsRead(int notificationId, int userId)
    {
        var affectedRows = _context.Database.ExecuteSqlRaw(
            @"UPDATE dbo.Notifications
              SET IsRead = 1
              WHERE Id = @NotificationId
                AND RecipientUserId = @UserId
                AND IsRead = 0",
            new SqlParameter("@NotificationId", notificationId),
            new SqlParameter("@UserId", userId));

        if (affectedRows > 0)
        {
            return true;
        }

        var existsForUser = _context.Database
            .SqlQueryRaw<int>(
                "SELECT COUNT(1) FROM dbo.Notifications WHERE Id = @NotificationId AND RecipientUserId = @UserId",
                new SqlParameter("@NotificationId", notificationId),
                new SqlParameter("@UserId", userId))
            .AsEnumerable()
            .FirstOrDefault() > 0;

        return existsForUser;
    }
}
