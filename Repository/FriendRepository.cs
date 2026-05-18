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
        return ExecuteStoredProcedure(
            "EXEC dbo.usp_SendFriendRequest @RequesterId, @ReceiverId",
            new SqlParameter("@RequesterId", requesterId),
            new SqlParameter("@ReceiverId", receiverId));
    }

    public int AcceptFriendRequest(int friendshipId, int receiverId)
    {
        return ExecuteStoredProcedure(
            "EXEC dbo.usp_AcceptFriendRequest @FriendshipId, @ReceiverId",
            new SqlParameter("@FriendshipId", friendshipId),
            new SqlParameter("@ReceiverId", receiverId));
    }

    public int RejectFriendRequest(int friendshipId, int receiverId)
    {
        return ExecuteStoredProcedure(
            "EXEC dbo.usp_RejectFriendRequest @FriendshipId, @ReceiverId",
            new SqlParameter("@FriendshipId", friendshipId),
            new SqlParameter("@ReceiverId", receiverId));
    }

    public int CancelFriendRequest(int friendshipId, int requesterId)
    {
        return ExecuteStoredProcedure(
            "EXEC dbo.usp_CancelFriendRequest @FriendshipId, @RequesterId",
            new SqlParameter("@FriendshipId", friendshipId),
            new SqlParameter("@RequesterId", requesterId));
    }

    public int RemoveFriend(int userId, int otherUserId)
    {
        return ExecuteStoredProcedure(
            "EXEC dbo.usp_RemoveFriend @UserId, @OtherUserId",
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

    private int ExecuteStoredProcedure(string sql, params SqlParameter[] parameters)
    {
        var returnParam = new SqlParameter
        {
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.ReturnValue
        };

        var sqlParams = new List<object> { returnParam };
        sqlParams.AddRange(parameters);

        _context.Database.ExecuteSqlRaw(sql, sqlParams);

        return returnParam.Value == DBNull.Value ? -1 : Convert.ToInt32(returnParam.Value);
    }
}
