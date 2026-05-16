CREATE OR ALTER PROCEDURE dbo.usp_GetOutgoingFriendRequests
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.Id AS FriendshipId,
        u.Id AS UserId,
        u.FirstName,
        u.LastName,
        f.CreatedAt
    FROM dbo.Friendships f
    INNER JOIN dbo.Users u ON u.Id = f.ReceiverId
    WHERE f.RequesterId = @UserId AND f.Status = 'Pending'
    ORDER BY f.CreatedAt DESC;
END
GO
