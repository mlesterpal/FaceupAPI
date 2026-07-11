IF OBJECT_ID(N'dbo.Conversations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Conversations
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        User1Id INT NOT NULL,
        User2Id INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Conversations_CreatedAt DEFAULT SYSUTCDATETIME(),
        LastMessageAt DATETIME2 NULL,
        CONSTRAINT CK_Conversations_DifferentUsers CHECK (User1Id <> User2Id),
        CONSTRAINT CK_Conversations_OrderedUsers CHECK (User1Id < User2Id),
        CONSTRAINT FK_Conversations_User1 FOREIGN KEY (User1Id) REFERENCES dbo.Users(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_Conversations_User2 FOREIGN KEY (User2Id) REFERENCES dbo.Users(Id) ON DELETE NO ACTION
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Conversations')
      AND name = N'UX_Conversations_UserPair'
)
BEGIN
    CREATE UNIQUE INDEX UX_Conversations_UserPair ON dbo.Conversations(User1Id, User2Id);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Conversations')
      AND name = N'IX_Conversations_User1_LastMessageAt'
)
BEGIN
    CREATE INDEX IX_Conversations_User1_LastMessageAt ON dbo.Conversations(User1Id, LastMessageAt DESC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Conversations')
      AND name = N'IX_Conversations_User2_LastMessageAt'
)
BEGIN
    CREATE INDEX IX_Conversations_User2_LastMessageAt ON dbo.Conversations(User2Id, LastMessageAt DESC);
END
GO
