CREATE OR ALTER PROCEDURE dbo.usp_GetConversationMessages
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

    SELECT
        m.Id AS MessageId,
        m.ConversationId,
        m.SenderUserId,
        sender.FirstName AS SenderFirstName,
        sender.LastName AS SenderLastName,
        sender.ProfilePicture AS SenderProfilePicture,
        m.Body,
        m.CreatedAt,
        m.IsRead
    FROM dbo.Messages m
    INNER JOIN dbo.Users sender ON sender.Id = m.SenderUserId
    WHERE m.ConversationId = @ConversationId
    ORDER BY m.CreatedAt ASC;
END
GO
