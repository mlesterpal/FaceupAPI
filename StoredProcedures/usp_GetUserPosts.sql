CREATE OR ALTER PROCEDURE dbo.usp_GetUserPosts
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id AS PostId,
        p.Message,
        p.ImageUrl,
        u.FirstName,
        u.ProfilePicture,
        p.CreatedAt,
        u.Id AS UserId
    FROM dbo.Posts p
    INNER JOIN dbo.Users u ON p.UserId = u.Id
    WHERE (@UserId IS NULL OR @UserId = 0 OR u.Id = @UserId)
    ORDER BY p.Id DESC;
END
GO
