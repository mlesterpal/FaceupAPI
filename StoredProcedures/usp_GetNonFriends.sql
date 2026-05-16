CREATE OR ALTER PROCEDURE dbo.usp_GetNonFriends
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.Id AS UserId,
        u.FirstName,
        u.LastName,
        u.Email
    FROM dbo.Users u
    WHERE u.Id <> @UserId
      AND NOT EXISTS (
          SELECT 1 FROM dbo.Friendships f
          WHERE f.Status IN ('Pending', 'Accepted')
            AND (
                  (f.RequesterId = @UserId AND f.ReceiverId = u.Id)
               OR (f.ReceiverId = @UserId AND f.RequesterId = u.Id)
            )
      )
    ORDER BY u.FirstName, u.LastName;
END
GO
