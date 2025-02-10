using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces.AI;
using TeamWeeklyStatus.Application.Interfaces.Services;
using TeamWeeklyStatus.Application.Services;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIEnginesController : ControllerBase
    {
        private readonly IAIEngineService _aiEngineService;

        public AIEnginesController(IAIEngineService aiEngineService)
        {
            _aiEngineService = aiEngineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAIEngines()
        {
            var engines = await _aiEngineService.GetAIEnginesAsync();
            return Ok(engines);
        }
    }
}
