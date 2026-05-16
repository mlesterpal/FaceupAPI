-- Manual test script for friend system (run after US14 + all usp_* procedures)
-- Assumes Users with Id 1 and 2 exist

DECLARE @rc INT;

-- 1. Send request: User 1 -> User 2
EXEC @rc = dbo.usp_SendFriendRequest @RequesterId = 1, @ReceiverId = 2;
SELECT @rc AS SendRequest_ReturnCode; -- expect 0

-- 2. Duplicate send should fail
EXEC @rc = dbo.usp_SendFriendRequest @RequesterId = 1, @ReceiverId = 2;
SELECT @rc AS DuplicateSend_ReturnCode; -- expect 2

-- 3. Self send should fail
EXEC @rc = dbo.usp_SendFriendRequest @RequesterId = 1, @ReceiverId = 1;
SELECT @rc AS SelfSend_ReturnCode; -- expect 3

-- 4. Accept (as user 2)
DECLARE @FriendshipId INT = (SELECT TOP 1 Id FROM dbo.Friendships WHERE RequesterId = 1 AND ReceiverId = 2 AND Status = 'Pending');
EXEC @rc = dbo.usp_AcceptFriendRequest @FriendshipId = @FriendshipId, @ReceiverId = 2;
SELECT @rc AS Accept_ReturnCode; -- expect 0

-- 5. Unfriend
EXEC @rc = dbo.usp_RemoveFriend @UserId = 1, @OtherUserId = 2;
SELECT @rc AS Unfriend_ReturnCode; -- expect 0

-- 6. Reject + resend flow (reset: send again, reject, resend)
EXEC @rc = dbo.usp_SendFriendRequest @RequesterId = 1, @ReceiverId = 2;
SET @FriendshipId = (SELECT TOP 1 Id FROM dbo.Friendships WHERE RequesterId = 1 AND ReceiverId = 2 ORDER BY Id DESC);
EXEC @rc = dbo.usp_RejectFriendRequest @FriendshipId = @FriendshipId, @ReceiverId = 2;
EXEC @rc = dbo.usp_SendFriendRequest @RequesterId = 1, @ReceiverId = 2;
SELECT @rc AS ResendAfterReject_ReturnCode; -- expect 0

SELECT * FROM dbo.Friendships ORDER BY Id DESC;
