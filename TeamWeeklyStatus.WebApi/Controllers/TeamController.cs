using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Exceptions;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.Services;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        [HttpGet("GetAll", Name = "GetAllTeams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }

        [HttpPost("Add", Name = "AddTeam")]
        public async Task<IActionResult> CreateTeam(TeamDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTeam = await _teamService.AddTeamAsync(request);
            return Ok(newTeam);
        }

        [HttpPut("Update", Name = "UpdateTeam")]
        public async Task<IActionResult> UpdateTeam([FromBody] TeamDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedTeam = await _teamService.UpdateTeamAsync(request);
                return Ok(updatedTeam);
            }
            catch (TeamNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("Delete", Name = "DeleteTeam")]
        public async Task<IActionResult> DeleteTeam([FromBody] TeamDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var deletedTeam = await _teamService.UpdateTeamAsync(request);
                return Ok(deletedTeam);
            }
            catch (TeamNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
