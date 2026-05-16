CREATE OR ALTER PROCEDURE dbo.usp_RejectFriendRequest
    @FriendshipId INT,
    @ReceiverId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.Friendships
        WHERE Id = @FriendshipId AND ReceiverId = @ReceiverId AND Status = 'Pending'
    )
        RETURN 1;

    UPDATE dbo.Friendships
    SET Status = 'Rejected', UpdatedAt = SYSUTCDATETIME()
    WHERE Id = @FriendshipId;

    RETURN 0;
END
GO
