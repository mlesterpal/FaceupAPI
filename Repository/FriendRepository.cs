using System.Data;
using Faceup.Models;
using Faceup.Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Faceup.Repository;

public class FriendRepository
{
    private readonly FaceupContext _context;

    public FriendRepository(FaceupContext context)
    {
        _context = context;
    }

    public int SendFriendRequest(int requesterId, int receiverId)
    {
        return ExecuteWithReturn(
            "EXEC @ret = dbo.usp_SendFriendRequest @RequesterId, @ReceiverId",
            new SqlParameter("@RequesterId", requesterId),
            new SqlParameter("@ReceiverId", receiverId));
    }

    public int AcceptFriendRequest(int friendshipId, int receiverId)
    {
        return ExecuteWithReturn(
            "EXEC @ret = dbo.usp_AcceptFriendRequest @FriendshipId, @ReceiverId",
            new SqlParameter("@FriendshipId", friendshipId),
            new SqlParameter("@ReceiverId", receiverId));
    }

    public int RejectFriendRequest(int friendshipId, int receiverId)
    {
        return ExecuteWithReturn(
            "EXEC @ret = dbo.usp_RejectFriendRequest @FriendshipId, @ReceiverId",
            new SqlParameter("@FriendshipId", friendshipId),
            new SqlParameter("@ReceiverId", receiverId));
    }

    public int CancelFriendRequest(int friendshipId, int requesterId)
    {
        return ExecuteWithReturn(
            "EXEC @ret = dbo.usp_CancelFriendRequest @FriendshipId, @RequesterId",
            new SqlParameter("@FriendshipId", friendshipId),
            new SqlParameter("@RequesterId", requesterId));
    }

    public int RemoveFriend(int userId, int otherUserId)
    {
        return ExecuteWithReturn(
            "EXEC @ret = dbo.usp_RemoveFriend @UserId, @OtherUserId",
            new SqlParameter("@UserId", userId),
            new SqlParameter("@OtherUserId", otherUserId));
    }

    public List<FriendUserResponse> GetFriends(int userId)
    {
        return _context.Database
            .SqlQueryRaw<FriendUserResponse>(
                "EXEC dbo.usp_GetFriends @UserId",
                new SqlParameter("@UserId", userId))
            .ToList();
    }

    public List<FriendRequestResponse> GetIncomingFriendRequests(int userId)
    {
        return _context.Database
            .SqlQueryRaw<FriendRequestResponse>(
                "EXEC dbo.usp_GetIncomingFriendRequests @UserId",
                new SqlParameter("@UserId", userId))
            .ToList();
    }

    public List<FriendRequestResponse> GetOutgoingFriendRequests(int userId)
    {
        return _context.Database
            .SqlQueryRaw<FriendRequestResponse>(
                "EXEC dbo.usp_GetOutgoingFriendRequests @UserId",
                new SqlParameter("@UserId", userId))
            .ToList();
    }

    public List<SuggestedUserResponse> GetNonFriends(int userId)
    {
        return _context.Database
            .SqlQueryRaw<SuggestedUserResponse>(
                "EXEC dbo.usp_GetNonFriends @UserId",
                new SqlParameter("@UserId", userId))
            .ToList();
    }

    private int ExecuteWithReturn(string sql, params SqlParameter[] parameters)
    {
        var returnParam = new SqlParameter("@ret", SqlDbType.Int)
        {
            Direction = ParameterDirection.ReturnValue
        };

        var allParams = new List<SqlParameter> { returnParam };
        allParams.AddRange(parameters);

        _context.Database.ExecuteSqlRaw(sql, allParams);

        return returnParam.Value is int code ? code : Convert.ToInt32(returnParam.Value);
    }
}
