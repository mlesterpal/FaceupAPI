using Faceup.Models.Dto;
using Faceup.Models.Response;
using Faceup.Repository;

namespace Faceup.Services;

public class NotificationService
{
    private readonly NotificationRepository _notificationRepository;
    private static readonly IReadOnlyDictionary<string, Func<string, string>> NotificationMessageTemplates =
        new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["PostLike"] = actorName => $"{actorName} liked your post.",
            ["PostComment"] = actorName => $"{actorName} commented on your post.",
            ["PostShare"] = actorName => $"{actorName} shared your post.",
            ["FriendRequestSent"] = actorName => $"{actorName} sent you a friend request.",
            ["FriendRequestAccepted"] = actorName => $"{actorName} accepted your friend request."
        };

    public NotificationService(NotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public void CreateNotification(CreateNotificationRequest request)
    {
        if (request.RecipientUserId <= 0)
        {
            throw new ArgumentException("RecipientUserId must be greater than zero.");
        }

        if (request.ActorUserId.HasValue && request.ActorUserId.Value <= 0)
        {
            throw new ArgumentException("ActorUserId must be greater than zero when provided.");
        }

        if (string.IsNullOrWhiteSpace(request.Type))
        {
            throw new ArgumentException("Notification type is required.");
        }

        if (request.ActorUserId.HasValue && request.ActorUserId.Value == request.RecipientUserId)
        {
            return;
        }

        var actorDisplayName = _notificationRepository.GetActorDisplayName(request.ActorUserId);
        var message = BuildMessage(request.Type, actorDisplayName);

        // Related entity metadata is persisted to support UI deep-linking.
        _notificationRepository.CreateNotification(request, message);
    }

    public List<UserNotificationResponse> GetNotifications(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("UserId must be greater than zero.");
        }

        return _notificationRepository.GetNotificationsByRecipientUserId(userId);
    }

    public MarkNotificationReadResponse MarkNotificationAsRead(int notificationId, int userId)
    {
        if (notificationId <= 0)
        {
            throw new ArgumentException("NotificationId must be greater than zero.");
        }

        if (userId <= 0)
        {
            throw new ArgumentException("UserId must be greater than zero.");
        }

        var markedAsRead = _notificationRepository.MarkAsRead(notificationId, userId);
        if (!markedAsRead)
        {
            throw new KeyNotFoundException("Notification not found for this user.");
        }

        return new MarkNotificationReadResponse
        {
            NotificationId = notificationId,
            UserId = userId,
            MarkedAsRead = true,
            Message = "Notification marked as read."
        };
    }

    private static string BuildMessage(string type, string actorDisplayName)
    {
        if (!NotificationMessageTemplates.TryGetValue(type, out var templateBuilder))
        {
            throw new ArgumentException($"Unsupported notification type '{type}'.");
        }

        return templateBuilder(actorDisplayName);
    }
}
