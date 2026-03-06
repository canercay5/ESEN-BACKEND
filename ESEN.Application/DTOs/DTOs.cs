using ESEN.Domain.Entities;
using System.Text.Json.Serialization;

namespace ESEN.Application.DTOs
{
    public class UserRegistrationDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("town")]
        public string Town { get; set; }

        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonPropertyName("device_token")]
        public string DeviceToken { get; set; }
    }

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

    public class PredictionRequestDto
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("town")]
        public string Town { get; set; }

        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonPropertyName("features")]
        public List<DailyDataDto> Features { get; set; } = new List<DailyDataDto>();
    }

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

    public class GenerateOutbreakRequestDto
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("town")]
        public string Town { get; set; }

        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonPropertyName("metrics")]
        public List<DailyHealthMetric> Metrics { get; set; }
    }

    public class UserInfoDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("surname")]
        public string Surname { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("town")]
        public string Town { get; set; }
        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }
        [JsonPropertyName("devicetoken")]
        public string DeviceToken { get; set; }
    }

    public class RegionDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("town")]
        public string Town { get; set; }
        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }
        [JsonPropertyName("normalizedDensity")]
        public double NormalizedDensity { get; set; }
    }
    public class LoginDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}