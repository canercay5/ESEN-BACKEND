using System.Text.Json.Serialization;

namespace ESEN.Domain.Entities
{
    public class PushNotification : BaseEntity
    {
        [JsonPropertyName("deviceToken")]
        public string DeviceToken { get; set; }
        [JsonPropertyName("userId")]
        public Guid UserId { get; private set; }
        [JsonPropertyName("outbreakAlertId")]
        public Guid? OutbreakAlertId { get; private set; }
        [JsonPropertyName("title")]
        public string Title { get; private set; }
        [JsonPropertyName("message")]
        public string Message { get; private set; }
        [JsonPropertyName("isSent")]
        public bool IsSent { get; private set; }
        [JsonPropertyName("sentAt")]
        public DateTime? SentAt { get; private set; }
        [JsonPropertyName("user")]
        public virtual User User { get; private set; }
        [JsonPropertyName("outbreakAlert")]
        public virtual OutbreakAlert OutbreakAlert { get; private set; }
        [JsonConstructor]
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