CREATE OR ALTER PROCEDURE dbo.usp_GetPostComments
    @PostId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.Id AS UserId,
        u.FirstName,
        u.LastName,
        u.ProfilePicture,
        c.Content,
        c.CreatedAt AS CommentedAt
    FROM dbo.Comments c
    INNER JOIN dbo.Users u ON u.Id = c.UserId
    WHERE c.PostId = @PostId
    ORDER BY c.CreatedAt DESC, u.Id DESC;
END
GO
