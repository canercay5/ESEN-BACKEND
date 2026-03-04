using ESEN.Domain.Entities;
using ESEN.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESEN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly IRepository<Region> _regionRepository;
        private readonly IRepository<DailyHealthMetric> _metricRepository;

        public SeedController(IRepository<Region> regionRepository, IRepository<DailyHealthMetric> metricRepository)
        {
            _regionRepository = regionRepository;
            _metricRepository = metricRepository;
        }

        [HttpPost("generate-outbreak")]
        public async Task<IActionResult> GenerateOutbreak([FromQuery] string city, [FromQuery] string town)
        {
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(town))
                return BadRequest("Şehir ve ilçe bilgisi boş olamaz.");

            city = city.ToUpper();
            town = town.ToUpper();

            var allRegions = await _regionRepository.GetAllAsync();
            var region = allRegions.FirstOrDefault(r => r.City == city && r.Town == town);

            if (region == null)
            {
                region = new Region(city, town, 0.8);
                await _regionRepository.AddAsync(region);
            }

            var today = DateTime.UtcNow.Date;
            var metrics = new List<DailyHealthMetric>
            {
                new DailyHealthMetric(region.Id, today.AddDays(-7), 10, 15.5, 60.0),
                new DailyHealthMetric(region.Id, today.AddDays(-6), 12, 14.2, 65.0),
                new DailyHealthMetric(region.Id, today.AddDays(-5), 25, 12.0, 70.0),
                new DailyHealthMetric(region.Id, today.AddDays(-4), 40, 10.5, 75.0),
                new DailyHealthMetric(region.Id, today.AddDays(-3), 45, 11.0, 72.0),
                new DailyHealthMetric(region.Id, today.AddDays(-2), 55, 9.5, 80.0),
                new DailyHealthMetric(region.Id, today.AddDays(-1), 60, 10.0, 78.0)
            };

            foreach (var metric in metrics)
            {
                await _metricRepository.AddAsync(metric);
            }

            return Ok(new
            {
                Message = $"{city} - {town} bölgesi için salgın verisi başarıyla üretildi ve veritabanına işlendi.",
                RegionId = region.Id,
                City = region.City,
                Town = region.Town
            });
        }
    }
}