CREATE OR ALTER PROCEDURE dbo.usp_GetUserPosts
    @UserId INT = NULL,
    @ViewerUserId INT = NULL
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
        u.Id AS UserId,
        COUNT(DISTINCT l.Id) AS LikeCount,
        CAST(MAX(CASE WHEN l.UserId = @ViewerUserId THEN 1 ELSE 0 END) AS bit) AS IsLiked,
        COUNT(DISTINCT s.Id) AS ShareCount,
        CAST(MAX(CASE WHEN s.UserId = @ViewerUserId THEN 1 ELSE 0 END) AS bit) AS IsShared,
        COUNT(DISTINCT c.Id) AS CommentCount
    FROM dbo.Posts p
    INNER JOIN dbo.Users u ON p.UserId = u.Id
    LEFT JOIN dbo.Likes l ON l.PostId = p.Id
    LEFT JOIN dbo.Shares s ON s.PostId = p.Id
    LEFT JOIN dbo.Comments c ON c.PostId = p.Id
    WHERE (@UserId IS NULL OR @UserId = 0 OR u.Id = @UserId)
    GROUP BY
        p.Id,
        p.Message,
        p.ImageUrl,
        u.FirstName,
        u.ProfilePicture,
        p.CreatedAt,
        u.Id
    ORDER BY p.Id DESC;
END
GO
