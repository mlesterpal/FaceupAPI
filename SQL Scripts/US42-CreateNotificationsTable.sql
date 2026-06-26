IF OBJECT_ID(N'dbo.Notifications', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Notifications
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        RecipientUserId INT NOT NULL,
        ActorUserId INT NULL,
        Type NVARCHAR(50) NOT NULL,
        Message NVARCHAR(255) NOT NULL,
        RelatedEntityType NVARCHAR(50) NULL,
        RelatedEntityId INT NULL,
        IsRead BIT NOT NULL CONSTRAINT DF_Notifications_IsRead DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Notifications_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Notifications_RecipientUser FOREIGN KEY (RecipientUserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Notifications_ActorUser FOREIGN KEY (ActorUserId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Notifications')
      AND name = N'IX_Notifications_RecipientUserId_CreatedAt'
)
BEGIN
    CREATE INDEX IX_Notifications_RecipientUserId_CreatedAt ON dbo.Notifications(RecipientUserId, CreatedAt DESC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Notifications')
      AND name = N'IX_Notifications_RecipientUserId_IsRead'
)
BEGIN
    CREATE INDEX IX_Notifications_RecipientUserId_IsRead ON dbo.Notifications(RecipientUserId, IsRead);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Notifications')
      AND name = N'IX_Notifications_ActorUserId'
)
BEGIN
    CREATE INDEX IX_Notifications_ActorUserId ON dbo.Notifications(ActorUserId);
END
GO
