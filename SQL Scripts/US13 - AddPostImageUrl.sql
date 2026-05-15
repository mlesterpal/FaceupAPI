-- Allow image-only posts
ALTER TABLE dbo.Posts
ALTER COLUMN Message NVARCHAR(MAX) NULL;

-- Optional image path/URL stored in DB
ALTER TABLE dbo.Posts
ADD ImageUrl NVARCHAR(500) NULL;
