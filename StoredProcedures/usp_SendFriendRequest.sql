CREATE OR ALTER PROCEDURE dbo.usp_SendFriendRequest
    @RequesterId INT,
    @ReceiverId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @RequesterId = @ReceiverId
        RETURN 3; -- invalid (self)

    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Id = @RequesterId)
        OR NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Id = @ReceiverId)
        RETURN 1; -- not found

    -- Already friends
    IF EXISTS (
        SELECT 1 FROM dbo.Friendships
        WHERE Status = 'Accepted'
          AND ((RequesterId = @RequesterId AND ReceiverId = @ReceiverId)
            OR (RequesterId = @ReceiverId AND ReceiverId = @RequesterId))
    )
        RETURN 2; -- duplicate / already friends

    -- They already sent you a pending request
    IF EXISTS (
        SELECT 1 FROM dbo.Friendships
        WHERE RequesterId = @ReceiverId AND ReceiverId = @RequesterId AND Status = 'Pending'
    )
        RETURN 4; -- incoming pending exists

    DECLARE @ExistingId INT;
    DECLARE @ExistingStatus NVARCHAR(20);

    SELECT @ExistingId = Id, @ExistingStatus = Status
    FROM dbo.Friendships
    WHERE RequesterId = @RequesterId AND ReceiverId = @ReceiverId;

    IF @ExistingId IS NOT NULL
    BEGIN
        IF @ExistingStatus = 'Rejected'
        BEGIN
            UPDATE dbo.Friendships
            SET Status = 'Pending', UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @ExistingId;
            RETURN 0;
        END

        IF @ExistingStatus IN ('Pending', 'Accepted')
            RETURN 2;

        -- Cancelled or Removed: allow new request by updating row
        IF @ExistingStatus IN ('Cancelled', 'Removed')
        BEGIN
            UPDATE dbo.Friendships
            SET Status = 'Pending', UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @ExistingId;
            RETURN 0;
        END
    END

    INSERT INTO dbo.Friendships (RequesterId, ReceiverId, Status)
    VALUES (@RequesterId, @ReceiverId, 'Pending');

    RETURN 0;
END
GO
