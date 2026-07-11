using Faceup.Models.Dto;
using Faceup.Models.Response;
using Faceup.Repository;

namespace Faceup.Services;

public class MessageService
{
    private const int MaxMessageLength = 2000;

    private readonly MessageRepository _messageRepository;
    private readonly NotificationService _notificationService;

    public MessageService(MessageRepository messageRepository, NotificationService notificationService)
    {
        _messageRepository = messageRepository;
        _notificationService = notificationService;
    }

    public List<ConversationListItemResponse> GetConversations(int userId)
    {
        ValidateUser(userId);
        return _messageRepository.GetConversations(userId);
    }

    public List<ConversationMessageResponse> GetConversationMessages(int conversationId, int userId)
    {
        ValidateConversationParticipant(conversationId, userId);
        return _messageRepository.GetConversationMessages(conversationId, userId);
    }

    public SendMessageResponse SendMessage(int userId, SendMessageRequest request)
    {
        ValidateUser(userId);

        if (request == null)
        {
            throw new ArgumentException("Message payload is required.");
        }

        var normalizedBody = NormalizeAndValidateBody(request.Body);
        int recipientUserId;

        if (request.ConversationId.HasValue)
        {
            var conversationId = request.ConversationId.Value;
            if (conversationId <= 0)
            {
                throw new ArgumentException("ConversationId must be greater than zero.");
            }

            var counterpartId = _messageRepository.GetCounterpartUserId(conversationId, userId);
            if (!counterpartId.HasValue || counterpartId.Value <= 0)
            {
                throw new KeyNotFoundException("Conversation not found for this user.");
            }

            recipientUserId = counterpartId.Value;
        }
        else
        {
            if (!request.RecipientUserId.HasValue || request.RecipientUserId.Value <= 0)
            {
                throw new ArgumentException("RecipientUserId is required when ConversationId is not provided.");
            }

            recipientUserId = request.RecipientUserId.Value;
        }

        if (recipientUserId == userId)
        {
            throw new ArgumentException("You cannot send a direct message to yourself.");
        }

        if (!_messageRepository.UserExists(recipientUserId))
        {
            throw new KeyNotFoundException("Recipient user not found.");
        }

        var response = _messageRepository.SendMessage(
            userId,
            request.ConversationId,
            recipientUserId,
            normalizedBody);

        _notificationService.CreateNotification(new CreateNotificationRequest
        {
            RecipientUserId = recipientUserId,
            ActorUserId = userId,
            Type = "DirectMessage",
            RelatedEntityType = "Conversation",
            RelatedEntityId = response.ConversationId
        });

        return response;
    }

    public MarkConversationReadResponse MarkConversationRead(int conversationId, int userId)
    {
        ValidateConversationParticipant(conversationId, userId);

        var markedCount = _messageRepository.MarkConversationRead(conversationId, userId);
        return new MarkConversationReadResponse
        {
            ConversationId = conversationId,
            UserId = userId,
            MarkedCount = markedCount
        };
    }

    public UnreadConversationCountResponse GetUnreadConversationCount(int userId)
    {
        ValidateUser(userId);

        return new UnreadConversationCountResponse
        {
            UnreadConversationCount = _messageRepository.GetUnreadConversationCount(userId)
        };
    }

    private void ValidateUser(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("UserId must be greater than zero.");
        }

        if (!_messageRepository.UserExists(userId))
        {
            throw new KeyNotFoundException("User not found.");
        }
    }

    private void ValidateConversationParticipant(int conversationId, int userId)
    {
        if (conversationId <= 0)
        {
            throw new ArgumentException("ConversationId must be greater than zero.");
        }

        ValidateUser(userId);

        if (!_messageRepository.IsConversationParticipant(conversationId, userId))
        {
            throw new KeyNotFoundException("Conversation not found for this user.");
        }
    }

    private static string NormalizeAndValidateBody(string? body)
    {
        var normalizedBody = body?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedBody))
        {
            throw new ArgumentException("Message body is required.");
        }

        if (normalizedBody.Length > MaxMessageLength)
        {
            throw new ArgumentException($"Message body must be at most {MaxMessageLength} characters.");
        }

        return normalizedBody;
    }
}
