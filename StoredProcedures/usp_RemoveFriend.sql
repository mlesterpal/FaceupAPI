CREATE OR ALTER PROCEDURE dbo.usp_RemoveFriend
    @UserId INT,
    @OtherUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @UserId = @OtherUserId
        RETURN 3;

    DELETE FROM dbo.Friendships
    WHERE Status = 'Accepted'
      AND ((RequesterId = @UserId AND ReceiverId = @OtherUserId)
        OR (RequesterId = @OtherUserId AND ReceiverId = @UserId));

    IF @@ROWCOUNT = 0
        RETURN 1;

    RETURN 0;
END
GO
