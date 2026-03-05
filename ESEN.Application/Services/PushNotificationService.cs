using ESEN.Application.Interfaces;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;

namespace ESEN.Infrastructure.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly ILogger<PushNotificationService> _logger;

        public PushNotificationService(ILogger<PushNotificationService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendNotificationAsync(string deviceToken, string title, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceToken) || deviceToken.Length < 10)
                {
                    _logger.LogWarning("Geçersiz veya boş cihaz token'ı. Bildirim gönderilmedi.");
                    return false;
                }

                var fcmMessage = new Message()
                {
                    Token = deviceToken,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = message
                    }
                };
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(fcmMessage);

                _logger.LogInformation("==================================================");
                _logger.LogInformation("🚀 FIREBASE PUSH NOTIFICATION BAŞARIYLA GÖNDERİLDİ!");
                _logger.LogInformation($"FCM Yanıtı: {response}");
                _logger.LogInformation($"Başlık: {title}");
                _logger.LogInformation("==================================================");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Firebase bildirimi gönderilirken kritik hata oluştu. Token: {deviceToken}");
                return false;
            }
        }
    }
}