using ESEN.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using ESEN.Domain.Entities;
using ESEN.Domain.Interfaces;

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

        [HttpGet("getinfo/{UserId}")]
        public async Task<IActionResult> GetInfo(Guid UserId)
        {
            var user = await _userRepository.GetByIdAsync(UserId);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(new { Message = "Kullanıcı bilgileri getirildi.", User = user});
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DeviceToken))
                return BadRequest("Device token boş olamaz.");

            if (string.IsNullOrWhiteSpace(dto.City) ||
                string.IsNullOrWhiteSpace(dto.Town) ||
                string.IsNullOrWhiteSpace(dto.Neighborhood))
                return BadRequest("Bölge bilgileri eksik.");

            var user = new User(dto.DeviceToken)
            {
                City = dto.City,
                Town = dto.Town,
                Neighborhood = dto.Neighborhood,
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                Password = dto.Password,
            };

            await _userRepository.AddAsync(user);

            return Ok(new { Message = "Kullanıcı başarıyla kaydedildi.", UserId = user.Id });
        }

        [HttpPost("{userId}/follow/{regionId}")]
        public async Task<IActionResult> FollowRegion(Guid userId, Guid regionId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            var region = await _regionRepository.GetByIdAsync(regionId);
            if (region == null) return NotFound("Bölge bulunamadı.");

            user.FollowRegion(region);
            await _userRepository.UpdateAsync(user);

            return Ok(new { Message = $"{region.Town} bölgesi takibe alındı." });
        }

        [HttpPost("{userId}/unfollow/{regionId}")]
        public async Task<IActionResult> UnfollowRegion(Guid userId, Guid regionId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            var region = await _regionRepository.GetByIdAsync(regionId);
            if (region == null) return NotFound("Bölge bulunamadı.");

            user.UnfollowRegion(region.Id);
            await _userRepository.UpdateAsync(user);

            return Ok(new { Message = $"{region.Town} bölgesi takipten çıkıldı." });
        }
    }
}
