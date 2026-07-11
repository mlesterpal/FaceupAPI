IF OBJECT_ID(N'dbo.Messages', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Messages
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ConversationId INT NOT NULL,
        SenderUserId INT NOT NULL,
        Body NVARCHAR(2000) NOT NULL,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Messages_CreatedAt DEFAULT SYSUTCDATETIME(),
        IsRead BIT NOT NULL CONSTRAINT DF_Messages_IsRead DEFAULT 0,
        CONSTRAINT FK_Messages_Conversation FOREIGN KEY (ConversationId) REFERENCES dbo.Conversations(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Messages_SenderUser FOREIGN KEY (SenderUserId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Messages')
      AND name = N'IX_Messages_ConversationId_CreatedAt'
)
BEGIN
    CREATE INDEX IX_Messages_ConversationId_CreatedAt ON dbo.Messages(ConversationId, CreatedAt DESC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Messages')
      AND name = N'IX_Messages_ConversationId_IsRead'
)
BEGIN
    CREATE INDEX IX_Messages_ConversationId_IsRead ON dbo.Messages(ConversationId, IsRead);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Messages')
      AND name = N'IX_Messages_SenderUserId'
)
BEGIN
    CREATE INDEX IX_Messages_SenderUserId ON dbo.Messages(SenderUserId);
END
GO
