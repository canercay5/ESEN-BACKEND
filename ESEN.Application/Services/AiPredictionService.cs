using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using ESEN.Application.DTOs;
using ESEN.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ESEN.Infrastructure.Services
{
    public class AiPredictionService : IAiPredictionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _aiApiUrl;

        public AiPredictionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _aiApiUrl = configuration["AiApiSettings:BaseUrl"] ?? "http://localhost:8000";
        }

        public async Task<PredictionResponseDto> CheckOutbreakRiskAsync(PredictionRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_aiApiUrl}/api/predict", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Yapay Zeka servisi hata döndürdü. Status Code: {response.StatusCode}, Detay: {errorContent}");
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = await response.Content.ReadFromJsonAsync<PredictionResponseDto>(options);

                if (result == null)
                {
                    throw new Exception("Yapay Zeka servisinden okunamayan veya boş bir cevap geldi.");
                }

                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Yapay Zeka (FastAPI) servisine ulaşılamıyor. Python API'nin terminalde çalışır durumda olduğundan emin olun.", ex);
            }
        }
    }
}