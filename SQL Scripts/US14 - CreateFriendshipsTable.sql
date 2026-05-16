CREATE TABLE dbo.Friendships (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RequesterId INT NOT NULL,
    ReceiverId INT NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Friendships_Requester FOREIGN KEY (RequesterId) REFERENCES dbo.Users(Id),
    CONSTRAINT FK_Friendships_Receiver FOREIGN KEY (ReceiverId) REFERENCES dbo.Users(Id),
    CONSTRAINT CK_Friendships_NotSelf CHECK (RequesterId <> ReceiverId),
    CONSTRAINT CK_Friendships_Status CHECK (Status IN ('Pending','Accepted','Rejected','Cancelled','Removed'))
);

CREATE UNIQUE INDEX UX_Friendships_RequesterReceiver
ON dbo.Friendships (RequesterId, ReceiverId)
WHERE Status IN ('Pending', 'Accepted');

CREATE INDEX IX_Friendships_Receiver_Status ON dbo.Friendships (ReceiverId, Status);
CREATE INDEX IX_Friendships_Requester_Status ON dbo.Friendships (RequesterId, Status);
