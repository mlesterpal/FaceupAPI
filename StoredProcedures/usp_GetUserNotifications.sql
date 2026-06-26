CREATE OR ALTER PROCEDURE dbo.usp_GetUserNotifications
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        n.Id AS NotificationId,
        n.RecipientUserId,
        n.ActorUserId,
        COALESCE(
            NULLIF(LTRIM(RTRIM(CONCAT(ISNULL(actor.FirstName, ''), ' ', ISNULL(actor.LastName, '')))), ''),
            actor.Email,
            'Someone'
        ) AS ActorName,
        actor.ProfilePicture AS ActorProfilePicture,
        n.Type,
        n.Message,
        n.CreatedAt,
        n.IsRead,
        n.RelatedEntityType,
        n.RelatedEntityId
    FROM dbo.Notifications n
    LEFT JOIN dbo.Users actor ON actor.Id = n.ActorUserId
    WHERE n.RecipientUserId = @UserId
    ORDER BY n.CreatedAt DESC;
END
GO
