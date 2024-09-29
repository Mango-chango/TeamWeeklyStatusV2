using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.Services;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.WebApi.DTOs;

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
        public async Task<IActionResult> CreateTeam(TeamPostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = new Team
            {
                Name = request.Name
            };

            var newTeam = await _teamService.AddTeamAsync(team);
            return Ok(newTeam);
        }

        [HttpPut("Update", Name = "UpdateTeam")]
        public async Task<IActionResult> UpdateTeam([FromBody] TeamPostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTeam = await _teamService.GetTeamByIdAsync(request.Id);
            if (existingTeam == null)
            {
                return NotFound();
            }

            existingTeam.Name = request.Name;

            var updatedTeam = await _teamService.UpdateTeamAsync(existingTeam);
            return Ok(updatedTeam);
        }

        [HttpDelete("Delete", Name = "DeleteTeam")]
        public async Task<IActionResult> DeleteTeam([FromBody] TeamPostRequest request)
        {
            var existingTeam = await _teamService.GetTeamByIdAsync(request.Id);
            if (existingTeam == null)
            {
                return NotFound();
            }

            var deletedTeam = await _teamService.DeleteTeamAsync(existingTeam);
            return Ok(deletedTeam);
        }
    }
}
