using System.Text.Json.Serialization;

namespace ESEN.Domain.Entities
{
    public class DailyHealthMetric : BaseEntity
    {
        [JsonPropertyName("regionId")]
        public Guid RegionId { get; private set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; private set; }
        [JsonPropertyName("totalSales")]
        public int TotalSales { get; private set; }
        [JsonPropertyName("averageTemperature")]
        public double AverageTemperature { get; private set; }
        [JsonPropertyName("averageHumidity")]
        public double AverageHumidity { get; private set; }
        [JsonPropertyName("region")]
        public virtual Region Region { get; private set; }

        [JsonConstructor]
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