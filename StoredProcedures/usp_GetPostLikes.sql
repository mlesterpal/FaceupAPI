CREATE OR ALTER PROCEDURE dbo.usp_GetPostLikes
    @PostId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.Id AS UserId,
        u.FirstName,
        u.LastName,
        u.ProfilePicture,
        l.CreatedAt AS LikedAt
    FROM dbo.Likes l
    INNER JOIN dbo.Users u ON u.Id = l.UserId
    WHERE l.PostId = @PostId
    ORDER BY l.CreatedAt DESC, u.Id DESC;
END
GO
