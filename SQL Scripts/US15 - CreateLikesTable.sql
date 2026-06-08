IF OBJECT_ID(N'dbo.Likes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Likes
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PostId INT NOT NULL,
        UserId INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Likes_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Likes_Posts FOREIGN KEY (PostId) REFERENCES dbo.Posts(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Likes_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Likes')
      AND name = N'UX_Likes_PostId_UserId'
)
BEGIN
    CREATE UNIQUE INDEX UX_Likes_PostId_UserId ON dbo.Likes(PostId, UserId);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Likes')
      AND name = N'IX_Likes_PostId'
)
BEGIN
    CREATE INDEX IX_Likes_PostId ON dbo.Likes(PostId);
END
GO
