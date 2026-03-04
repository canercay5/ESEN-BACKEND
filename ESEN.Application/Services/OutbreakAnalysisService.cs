using ESEN.Application.DTOs;
using ESEN.Application.Interfaces;
using ESEN.Domain.Entities;
using ESEN.Domain.Interfaces;

namespace ESEN.Application.Services
{
    public class OutbreakAnalysisService
    {
        private readonly IRepository<Region> _regionRepository;
        private readonly IRepository<DailyHealthMetric> _metricRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<OutbreakAlert> _alertRepository;
        private readonly IRepository<PushNotification> _notificationRepository;

        private readonly IAiPredictionService _aiPredictionService;
        private readonly IPushNotificationService _pushNotificationService;

        public OutbreakAnalysisService(
            IRepository<Region> regionRepository,
            IRepository<DailyHealthMetric> metricRepository,
            IRepository<User> userRepository,
            IRepository<OutbreakAlert> alertRepository,
            IRepository<PushNotification> notificationRepository,
            IAiPredictionService aiPredictionService,
            IPushNotificationService pushNotificationService)
        {
            _regionRepository = regionRepository;
            _metricRepository = metricRepository;
            _userRepository = userRepository;
            _alertRepository = alertRepository;
            _notificationRepository = notificationRepository;
            _aiPredictionService = aiPredictionService;
            _pushNotificationService = pushNotificationService;
        }

        public async Task AnalyzeRegionAsync(Guid regionId)
        {
            // 1. Bölgeyi getir
            var region = await _regionRepository.GetByIdAsync(regionId);
            if (region == null) throw new Exception("Bölge bulunamadı.");

            // 2. Son 7 günün verilerini MongoDB'den getir (Gerçekte veritabanı sorgusuyla filtrelenmeli)
            var allMetrics = await _metricRepository.GetAllAsync();
            var last7DaysMetrics = allMetrics
                .Where(m => m.RegionId == regionId)
                .OrderByDescending(m => m.Date)
                .Take(7)
                .OrderBy(m => m.Date) // API'ye kronolojik sırayla gitmeli
                .ToList();

            if (last7DaysMetrics.Count < 7)
                throw new Exception("Yapay zeka analizi için yeterli geçmiş veri yok (Min: 7 gün).");

            // 3. Python API için DTO oluştur
            var requestDto = new PredictionRequestDto
            {
                City = region.City,
                Town = region.Town,
                Features = last7DaysMetrics.Select(m => new DailyDataDto
                {
                    Sales = m.TotalSales,
                    Avg_Temp = m.AverageTemperature,
                    Avg_Humidity = m.AverageHumidity,
                    NormalizedDensity = region.NormalizedDensity
                }).ToList()
            };

            // 4. FastAPI'ye istek at (Köprüden geçiş)
            var aiResult = await _aiPredictionService.CheckOutbreakRiskAsync(requestDto);

            // 5. Eğer salgın riski varsa!
            if (aiResult.Is_Outbreak_Detected)
            {
                // Alarmı veritabanına kaydet
                var alert = new OutbreakAlert(region.Id, DateTime.UtcNow, aiResult.Risk_Score, aiResult.Threshold, true, aiResult.Message);
                await _alertRepository.AddAsync(alert);

                // Bu bölgeyi takip eden kullanıcıları bul
                var allUsers = await _userRepository.GetAllAsync();
                var usersToNotify = allUsers.Where(u => u.FollowedRegions.Any(r => r.RegionId == region.Id)).ToList();

                // Flutter (Firebase) üzerinden kullanıcılara bildirim at
                foreach (var user in usersToNotify)
                {
                    string title = $"⚠️ Salgın Riski: {region.Town}";
                    string message = $"Modelimiz {region.Town} bölgesi için anormal sağlık hareketliliği tespit etti. Lütfen dikkatli olun.";

                    bool isSent = await _pushNotificationService.SendNotificationAsync(user.DeviceToken, title, message);

                    // Bildirim geçmişini kaydet
                    var notificationRecord = new PushNotification(user.Id, alert.Id, title, message);
                    if (isSent) notificationRecord.MarkAsSent();

                    await _notificationRepository.AddAsync(notificationRecord);
                }
            }
        }
    }
}