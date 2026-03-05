using ESEN.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using ESEN.Domain.Entities;
using ESEN.Domain.Interfaces;

namespace ESEN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<PushNotification> _notifiyRepository;

        public NotificationController(IRepository<User> userRepository, IRepository<PushNotification> notifiyRepository)
        {
            _userRepository = userRepository;
            _notifiyRepository = notifiyRepository;
        }

        [HttpGet("get/{UserId}")]
        public async Task<IActionResult> GetNotifications(Guid UserId)
        {
            var allNotifications = await _notifiyRepository.GetAllAsync();
            var userNotifications = allNotifications.Where(n => n.UserId == UserId).ToList();

            return Ok(new { Message = "Bildirimler getirildi.", Notifications = userNotifications });
        }
    }
}
