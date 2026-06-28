namespace Faceup.Models.Response;

public class MarkNotificationReadResponse
{
    public int NotificationId { get; set; }
    public int UserId { get; set; }
    public bool MarkedAsRead { get; set; }
    public string Message { get; set; } = string.Empty;
}
