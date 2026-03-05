namespace ESEN.Domain.Entities
{
    public class Region : BaseEntity
    {
        public string City { get; private set; }
        public string Town { get; private set; }
        public double NormalizedDensity { get; private set; }
        public string Neighborhood { get; private set; }
        public Region(string city, string town, string neigborhood, double normalizedDensity)
        {
            City = city;
            Town = town;
            NormalizedDensity = normalizedDensity;
            Neighborhood = neigborhood;
        }
        public virtual ICollection<DailyHealthMetric> DailyMetrics { get; private set; } = new List<DailyHealthMetric>();
        public virtual ICollection<OutbreakAlert> OutbreakAlerts { get; private set; } = new List<OutbreakAlert>();
    }
}