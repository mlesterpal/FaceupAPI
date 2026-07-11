CREATE OR ALTER PROCEDURE dbo.usp_GetUnreadConversationCount
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(DISTINCT m.ConversationId) AS UnreadConversationCount
    FROM dbo.Messages m
    INNER JOIN dbo.Conversations c ON c.Id = m.ConversationId
    WHERE m.IsRead = 0
      AND m.SenderUserId <> @UserId
      AND (c.User1Id = @UserId OR c.User2Id = @UserId);
END
GO
