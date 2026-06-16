IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Comments')
      AND name = N'IX_Comments_PostId_UserId'
)
BEGIN
    DROP INDEX IX_Comments_PostId_UserId ON dbo.Comments;
END
GO
