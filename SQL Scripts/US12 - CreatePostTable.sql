CREATE TABLE Posts
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Message NVARCHAR(MAX) NOT NULL,

    -- Foreign Key
    UserId INT NOT NULL,

    -- Relationship
    CONSTRAINT FK_Posts_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE
);