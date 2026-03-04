using ESEN.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ESEN.Infrastructure.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        // Sistem loglarını konsola veya dosyaya yazdırmak için .NET'in yerleşik Logger'ını kullanıyoruz
        private readonly ILogger<PushNotificationService> _logger;

        public PushNotificationService(ILogger<PushNotificationService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendNotificationAsync(string deviceToken, string title, string message)
        {
            try
            {
                // TODO: İleride Firebase Admin SDK (FirebaseApp.DefaultInstance) entegrasyonu buraya gelecek.
                /* Gerçek Kod Örneği:
                 * var fcmMessage = new Message() { Token = deviceToken, Notification = new Notification { Title = title, Body = message } };
                 * string response = await FirebaseMessaging.DefaultInstance.SendAsync(fcmMessage);
                 */

                // Şimdilik gönderim işlemini simüle ediyoruz (Mocking)
                _logger.LogInformation("==================================================");
                _logger.LogInformation("📱 PUSH NOTIFICATION GÖNDERİLİYOR...");
                _logger.LogInformation($"Hedef Cihaz: {deviceToken}");
                _logger.LogInformation($"Başlık: {title}");
                _logger.LogInformation($"Mesaj: {message}");
                _logger.LogInformation("Durum: BAŞARILI");
                _logger.LogInformation("==================================================");

                // İşlemin başarılı olduğunu simüle etmek için küçük bir gecikme ekliyoruz
                await Task.Delay(100);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Bildirim gönderilirken hata oluştu. Cihaz: {deviceToken}");
                return false;
            }
        }
    }
}