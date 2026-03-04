namespace ESEN.Domain.Entities
{
    public class PushNotification : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid? OutbreakAlertId { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }

        public bool IsSent { get; private set; }
        public DateTime? SentAt { get; private set; }

        public virtual User User { get; private set; }
        public virtual OutbreakAlert OutbreakAlert { get; private set; }

        public PushNotification(Guid userId, Guid? outbreakAlertId, string title, string message)
        {
            UserId = userId;
            OutbreakAlertId = outbreakAlertId;
            Title = title;
            Message = message;
            IsSent = false;
        }

        public void MarkAsSent()
        {
            IsSent = true;
            SentAt = DateTime.UtcNow;
        }
    }
}