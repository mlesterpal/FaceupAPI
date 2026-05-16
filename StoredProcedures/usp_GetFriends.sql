CREATE OR ALTER PROCEDURE dbo.usp_GetFriends
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.Id AS UserId,
        u.FirstName,
        u.LastName,
        f.Id AS FriendshipId,
        f.UpdatedAt AS FriendsSince
    FROM dbo.Friendships f
    INNER JOIN dbo.Users u ON u.Id = CASE WHEN f.RequesterId = @UserId THEN f.ReceiverId ELSE f.RequesterId END
    WHERE f.Status = 'Accepted'
      AND (f.RequesterId = @UserId OR f.ReceiverId = @UserId)
    ORDER BY f.UpdatedAt DESC;
END
GO
