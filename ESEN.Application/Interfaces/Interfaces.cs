using ESEN.Application.DTOs;

namespace ESEN.Application.Interfaces
{
    public interface IAiPredictionService
    {
        Task<PredictionResponseDto> CheckOutbreakRiskAsync(PredictionRequestDto request);
    }

    public interface IPushNotificationService
    {
        Task<bool> SendNotificationAsync(string deviceToken, string title, string message);
    }
}