namespace ESEN.Domain.Entities
{
    public class OutbreakAlert : BaseEntity
    {
        public Guid RegionId { get; private set; }
        public DateTime TargetDate { get; private set; }
        public double RiskScore { get; private set; }
        public double Threshold { get; private set; }
        public bool IsOutbreakDetected { get; private set; }
        public string AlertMessage { get; private set; }

        public virtual Region Region { get; private set; }

        public OutbreakAlert(Guid regionId, DateTime targetDate, double riskScore, double threshold, bool isOutbreakDetected, string alertMessage)
        {
            RegionId = regionId;
            TargetDate = targetDate;
            RiskScore = riskScore;
            Threshold = threshold;
            IsOutbreakDetected = isOutbreakDetected;
            AlertMessage = alertMessage;
        }
    }
}