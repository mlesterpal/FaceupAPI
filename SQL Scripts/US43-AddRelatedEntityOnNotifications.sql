IF COL_LENGTH('dbo.Notifications', 'RelatedEntityType') IS NULL
BEGIN
    ALTER TABLE dbo.Notifications
    ADD RelatedEntityType NVARCHAR(50) NULL;
END
GO

IF COL_LENGTH('dbo.Notifications', 'RelatedEntityId') IS NULL
BEGIN
    ALTER TABLE dbo.Notifications
    ADD RelatedEntityId INT NULL;
END
GO
