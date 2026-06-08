IF OBJECT_ID(N'dbo.Shares', N'U') IS NULL
BEGIN
    CREATE TABLE Shares (
        Id INT PRIMARY KEY IDENTITY(1,1),
        PostId INT NOT NULL,
        UserId INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Shares_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Shares_PostId FOREIGN KEY (PostId) REFERENCES Posts(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Shares_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION,
    );
END
GO

IF NOT EXISTS(
    SELECT 1
    FROM sys.indexes
    WHERE Object_ID = OBJECT_ID('Shares')
     AND Name = 'IX_Shares_PostId_UserId'
)
BEGIN
    CREATE UNIQUE INDEX IX_Shares_PostId_UserId ON Shares(PostId, UserId);
END
GO

IF NOT EXISTS(
    SELECT 1
    FROM sys.indexes
    WHERE Object_ID = OBJECT_ID('Shares')
     AND Name = 'IX_Shares_PostId'
)
BEGIN
    CREATE UNIQUE INDEX IX_Shares_PostId ON Shares(PostId);
END
GO