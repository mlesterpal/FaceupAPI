CREATE OR ALTER PROCEDURE dbo.usp_GetUserPosts
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id AS PostId,
        p.Message,
        p.ImageUrl,
        u.FirstName,
        u.ProfilePicture,
        p.CreatedAt
    FROM dbo.Posts p
    INNER JOIN dbo.Users u ON p.UserId = u.Id
    WHERE u.Id = @UserId
    ORDER BY p.Id DESC;
END
GO
