using ESEN.Domain.Entities;
using ESEN.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESEN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Region> _regionRepository;

        public UserController(IRepository<User> userRepository, IRepository<Region> regionRepository)
        {
            _userRepository = userRepository;
            _regionRepository = regionRepository;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] string deviceToken)
        {
            if (string.IsNullOrWhiteSpace(deviceToken))
                return BadRequest("Device token boş olamaz.");

            var user = new User(deviceToken);
            await _userRepository.AddAsync(user);

            return Ok(new { Message = "Kullanıcı cihazı başarıyla kaydedildi.", UserId = user.Id });
        }

        // POST: api/user/{userId}/follow/{regionId}
        [HttpPost("{userId}/follow/{regionId}")]
        public async Task<IActionResult> FollowRegion(Guid userId, Guid regionId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            var region = await _regionRepository.GetByIdAsync(regionId);
            if (region == null) return NotFound("Bölge bulunamadı.");

            // DDD mantığı: Nesnenin kendi davranışını tetikliyoruz
            user.FollowRegion(region);

            // Veritabanını güncelliyoruz
            await _userRepository.UpdateAsync(user);

            return Ok(new { Message = $"{region.Town} bölgesi takibe alındı." });
        }
    }
}