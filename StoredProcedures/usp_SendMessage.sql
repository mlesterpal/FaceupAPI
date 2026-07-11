CREATE OR ALTER PROCEDURE dbo.usp_SendMessage
    @UserId INT,
    @ConversationId INT = NULL,
    @RecipientUserId INT = NULL,
    @Body NVARCHAR(2000)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Body = LTRIM(RTRIM(@Body));

    IF @UserId <= 0
    BEGIN
        RAISERROR('UserId must be greater than zero.', 16, 1);
        RETURN;
    END

    IF @Body IS NULL OR LEN(@Body) = 0
    BEGIN
        RAISERROR('Message body is required.', 16, 1);
        RETURN;
    END

    IF @ConversationId IS NOT NULL
    BEGIN
        IF NOT EXISTS (
            SELECT 1
            FROM dbo.Conversations c
            WHERE c.Id = @ConversationId
              AND (c.User1Id = @UserId OR c.User2Id = @UserId)
        )
        BEGIN
            RAISERROR('Conversation not found for this user.', 16, 1);
            RETURN;
        END

        SELECT @RecipientUserId =
            CASE
                WHEN c.User1Id = @UserId THEN c.User2Id
                ELSE c.User1Id
            END
        FROM dbo.Conversations c
        WHERE c.Id = @ConversationId;
    END
    ELSE
    BEGIN
        IF @RecipientUserId IS NULL OR @RecipientUserId <= 0
        BEGIN
            RAISERROR('RecipientUserId is required when ConversationId is not provided.', 16, 1);
            RETURN;
        END

        IF @RecipientUserId = @UserId
        BEGIN
            RAISERROR('Cannot send message to yourself.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Id = @RecipientUserId)
        BEGIN
            RAISERROR('Recipient user not found.', 16, 1);
            RETURN;
        END

        DECLARE @User1Id INT = CASE WHEN @UserId < @RecipientUserId THEN @UserId ELSE @RecipientUserId END;
        DECLARE @User2Id INT = CASE WHEN @UserId < @RecipientUserId THEN @RecipientUserId ELSE @UserId END;

        SELECT @ConversationId = c.Id
        FROM dbo.Conversations c
        WHERE c.User1Id = @User1Id
          AND c.User2Id = @User2Id;

        IF @ConversationId IS NULL
        BEGIN
            INSERT INTO dbo.Conversations (User1Id, User2Id, LastMessageAt)
            VALUES (@User1Id, @User2Id, SYSUTCDATETIME());

            SET @ConversationId = SCOPE_IDENTITY();
        END
    END

    INSERT INTO dbo.Messages (ConversationId, SenderUserId, Body)
    VALUES (@ConversationId, @UserId, @Body);

    DECLARE @MessageId INT = SCOPE_IDENTITY();
    DECLARE @CreatedAt DATETIME2;

    SELECT @CreatedAt = m.CreatedAt
    FROM dbo.Messages m
    WHERE m.Id = @MessageId;

    UPDATE dbo.Conversations
    SET LastMessageAt = @CreatedAt
    WHERE Id = @ConversationId;

    SELECT
        @MessageId AS MessageId,
        @ConversationId AS ConversationId,
        @UserId AS SenderUserId,
        @RecipientUserId AS RecipientUserId,
        @Body AS Body,
        @CreatedAt AS CreatedAt;
END
GO
