using System.Text.Json.Serialization; // Bunu eklemeyi unutma!

namespace ESEN.Application.DTOs
{
    // C#'tan Python'a gidecek günlük veri modeli
    public class DailyDataDto
    {
        [JsonPropertyName("sales")]
        public int Sales { get; set; }

        [JsonPropertyName("avg_temp")]
        public double Avg_Temp { get; set; }

        [JsonPropertyName("avg_humidity")]
        public double Avg_Humidity { get; set; }

        [JsonPropertyName("normalizedDensity")]
        public double NormalizedDensity { get; set; }
    }

    // Python API'sine atılacak ana istek modeli
    public class PredictionRequestDto
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("town")]
        public string Town { get; set; }

        [JsonPropertyName("features")]
        public List<DailyDataDto> Features { get; set; } = new List<DailyDataDto>();
    }

    // Python'dan C#'a dönecek cevap modeli
    public class PredictionResponseDto
    {
        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("risk_score")]
        public double Risk_Score { get; set; }

        [JsonPropertyName("threshold")]
        public double Threshold { get; set; }

        [JsonPropertyName("is_outbreak_detected")]
        public bool Is_Outbreak_Detected { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}