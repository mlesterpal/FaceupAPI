CREATE OR ALTER PROCEDURE dbo.usp_GetUserConversations
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id AS ConversationId,
        counterpart.Id AS OtherUserId,
        counterpart.FirstName AS OtherUserFirstName,
        counterpart.LastName AS OtherUserLastName,
        counterpart.ProfilePicture AS OtherUserProfilePicture,
        lastMessage.Id AS LastMessageId,
        lastMessage.SenderUserId AS LastMessageSenderUserId,
        lastMessage.Body AS LastMessageBody,
        lastMessage.CreatedAt AS LastMessageCreatedAt,
        c.LastMessageAt,
        unread.UnreadCount
    FROM dbo.Conversations c
    INNER JOIN dbo.Users counterpart
        ON counterpart.Id = CASE
            WHEN c.User1Id = @UserId THEN c.User2Id
            ELSE c.User1Id
        END
    OUTER APPLY (
        SELECT TOP 1 m.Id, m.SenderUserId, m.Body, m.CreatedAt
        FROM dbo.Messages m
        WHERE m.ConversationId = c.Id
        ORDER BY m.CreatedAt DESC
    ) AS lastMessage
    OUTER APPLY (
        SELECT COUNT(1) AS UnreadCount
        FROM dbo.Messages m
        WHERE m.ConversationId = c.Id
          AND m.SenderUserId <> @UserId
          AND m.IsRead = 0
    ) AS unread
    WHERE c.User1Id = @UserId OR c.User2Id = @UserId
    ORDER BY COALESCE(c.LastMessageAt, c.CreatedAt) DESC;
END
GO
