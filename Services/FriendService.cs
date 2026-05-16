using Faceup.Models.Response;
using Faceup.Repository;

namespace Faceup.Services;

public class FriendService
{
    private readonly FriendRepository _friendRepository;

    public FriendService(FriendRepository friendRepository)
    {
        _friendRepository = friendRepository;
    }

    public int SendFriendRequest(int requesterId, int receiverId) =>
        _friendRepository.SendFriendRequest(requesterId, receiverId);

    public int AcceptFriendRequest(int friendshipId, int receiverId) =>
        _friendRepository.AcceptFriendRequest(friendshipId, receiverId);

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
