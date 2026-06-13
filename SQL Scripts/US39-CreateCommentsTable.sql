CREATE TABLE Comments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Comments_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Comments_Posts FOREIGN KEY (PostId) REFERENCES Posts(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Comments_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION,
);

IF NOT EXISTS(
    SELECT 1
    FROM sys.indexes
    WHERE Object_ID = OBJECT_ID('Comments')
     AND Name = 'IX_Comments_PostId_UserId'
)
BEGIN
    CREATE UNIQUE INDEX IX_Comments_PostId_UserId ON Comments(PostId, UserId);
END
GO

IF NOT EXISTS(
    SELECT 1
    FROM sys.indexes
    WHERE Object_ID = OBJECT_ID('Comments')
     AND Name = 'IX_Comments_PostId'        
)
 BEGIN
    CREATE INDEX IX_Comments_PostId ON Comments(PostId);
 END
 GO