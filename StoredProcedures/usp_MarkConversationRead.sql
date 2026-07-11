CREATE OR ALTER PROCEDURE dbo.usp_MarkConversationRead
    @ConversationId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

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

    UPDATE dbo.Messages
    SET IsRead = 1
    WHERE ConversationId = @ConversationId
      AND SenderUserId <> @UserId
      AND IsRead = 0;

    SELECT @@ROWCOUNT AS MarkedCount;
END
GO
