namespace ESEN.Domain.Entities
{
    public class DailyHealthMetric : BaseEntity
    {
        public Guid RegionId { get; private set; }
        public DateTime Date { get; private set; }
        public int TotalSales { get; private set; }
        public double AverageTemperature { get; private set; }
        public double AverageHumidity { get; private set; }

        public virtual Region Region { get; private set; }

        public DailyHealthMetric(Guid regionId, DateTime date, int totalSales, double avgTemp, double avgHumidity)
        {
            RegionId = regionId;
            Date = date;
            TotalSales = totalSales;
            AverageTemperature = avgTemp;
            AverageHumidity = avgHumidity;
        }
    }
}