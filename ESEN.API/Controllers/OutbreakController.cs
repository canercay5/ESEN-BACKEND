using ESEN.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESEN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutbreakController : ControllerBase
    {
        private readonly OutbreakAnalysisService _outbreakAnalysisService;

        public OutbreakController(OutbreakAnalysisService outbreakAnalysisService)
        {
            _outbreakAnalysisService = outbreakAnalysisService;
        }

        // POST: api/outbreak/analyze/{regionId}
        [HttpPost("analyze/{regionId}")]
        public async Task<IActionResult> AnalyzeRegion(Guid regionId)
        {
            try
            {
                // Application katmanýndaki koca iþ mantýðýný tek satýrda tetikliyoruz. (DDD'nin gücü!)
                await _outbreakAnalysisService.AnalyzeRegionAsync(regionId);
                
                return Ok(new { 
                    IsSuccess = true, 
                    Message = "Bölge analizi baþarýyla tamamlandý. Salgýn riski varsa takipçilere bildirim gönderildi." 
                });
            }
            catch (Exception ex)
            {
                // Python API kapalýysa veya geçmiþ 7 günlük veri yoksa buraya düþer
                return BadRequest(new { 
                    IsSuccess = false, 
                    Error = ex.Message 
                });
            }
        }
    }
}