CREATE OR ALTER PROCEDURE dbo.usp_CancelFriendRequest
    @FriendshipId INT,
    @RequesterId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.Friendships
        WHERE Id = @FriendshipId AND RequesterId = @RequesterId AND Status = 'Pending'
    )
        RETURN 1;

    UPDATE dbo.Friendships
    SET Status = 'Cancelled', UpdatedAt = SYSUTCDATETIME()
    WHERE Id = @FriendshipId;

    RETURN 0;
END
GO
