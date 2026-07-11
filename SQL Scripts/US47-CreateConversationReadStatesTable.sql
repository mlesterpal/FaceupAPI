                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           IF OBJECT_ID(N'dbo.ConversationReadStates', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ConversationReadStates
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ConversationId INT NOT NULL,
        UserId INT NOT NULL,
        LastReadMessageId INT NULL,
        UpdatedAt DATETIME2 NOT NULL CONSTRAINT DF_ConversationReadStates_UpdatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_ConversationReadStates_Conversation FOREIGN KEY (ConversationId) REFERENCES dbo.Conversations(Id) ON DELETE CASCADE,
        CONSTRAINT FK_ConversationReadStates_User FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
        CONSTRAINT FK_ConversationReadStates_LastReadMessage FOREIGN KEY (LastReadMessageId) REFERENCES dbo.Messages(Id) ON DELETE NO ACTION
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.ConversationReadStates')
      AND name = N'UX_ConversationReadStates_Conversation_User'
)
BEGIN
    CREATE UNIQUE INDEX UX_ConversationReadStates_Conversation_User ON dbo.ConversationReadStates(ConversationId, UserId);
END
GO
