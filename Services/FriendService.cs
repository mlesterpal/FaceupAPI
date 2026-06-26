using Faceup.Models.Dto;
using Faceup.Models.Response;
using Faceup.Repository;

namespace Faceup.Services;

public class FriendService
{
    private readonly FriendRepository _friendRepository;
    private readonly NotificationService _notificationService;

    public FriendService(FriendRepository friendRepository, NotificationService notificationService)
    {
        _friendRepository = friendRepository;
        _notificationService = notificationService;
    }

    public int SendFriendRequest(int requesterId, int receiverId)
    {
        var resultCode = _friendRepository.SendFriendRequest(requesterId, receiverId);
        if (resultCode == 0)
        {
            _notificationService.CreateNotification(new CreateNotificationRequest
            {
                RecipientUserId = receiverId,
                ActorUserId = requesterId,
                Type = "FriendRequestSent"
            });
        }

        return resultCode;
    }

    public int AcceptFriendRequest(int friendshipId, int receiverId)
    {
        var friendshipParticipants = _friendRepository.GetFriendshipParticipants(friendshipId);
        var resultCode = _friendRepository.AcceptFriendRequest(friendshipId, receiverId);

        if (resultCode == 0)
        {
            _notificationService.CreateNotification(new CreateNotificationRequest
            {
                RecipientUserId = friendshipParticipants.RequesterId,
                ActorUserId = receiverId,
                Type = "FriendRequestAccepted",
                RelatedEntityId = friendshipId
            });
        }

        return resultCode;
    }

    public int RejectFriendRequest(int friendshipId, int receiverId) =>
        _friendRepository.RejectFriendRequest(friendshipId, receiverId);

    public int CancelFriendRequest(int friendshipId, int requesterId) =>
        _friendRepository.CancelFriendRequest(friendshipId, requesterId);

    public int RemoveFriend(int userId, int otherUserId) =>
        _friendRepository.RemoveFriend(userId, otherUserId);

    public List<FriendUserResponse> GetFriends(int userId) =>
        _friendRepository.GetFriends(userId);

    public List<FriendRequestResponse> GetIncomingFriendRequests(int userId) =>
        _friendRepository.GetIncomingFriendRequests(userId);

    public List<FriendRequestResponse> GetOutgoingFriendRequests(int userId) =>
        _friendRepository.GetOutgoingFriendRequests(userId);

    public List<SuggestedUserResponse> GetNonFriends(int userId) =>
        _friendRepository.GetNonFriends(userId);

    public static string? GetErrorMessage(int code) => code switch
    {
        0 => null,
        1 => "Friend request or user not found.",
        2 => "You are already friends or a request already exists.",
        3 => "You cannot send a friend request to yourself.",
        4 => "This user already sent you a friend request. Accept it instead.",
        _ => "An unexpected error occurred."
    };
}
