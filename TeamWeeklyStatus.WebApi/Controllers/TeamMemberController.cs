using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly ITeamMemberService _teamMemberService;
        public TeamMemberController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllTeamMembers([FromBody] TeamMemberDTO request)
        {
            var members = await _teamMemberService.GetAllTeamMembersAsync((int)request.TeamId);
            if (members == null)
            {
                return NotFound();
            }
            return Ok(members);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddTeamMember([FromBody] TeamMemberDTO request)
        {
            var addedTeamMember = await _teamMemberService.AddTeamMemberAsync(request);
            return Ok(addedTeamMember);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateTeamMember([FromBody] TeamMemberDTO request)
        {
            var updatedTeamMember = await _teamMemberService.UpdateTeamMemberAsync(request);
            return Ok(updatedTeamMember);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> RemoveTeamMember([FromQuery] int teamId, [FromQuery] int memberId)
        {
            var teamMemberDto = new TeamMemberDTO
            {
                TeamId = teamId,
                MemberId = memberId,
            };
            await _teamMemberService.DeleteTeamMemberAsync(teamMemberDto);
            return Ok();
        }

        [HttpPost("AssignCurrentWeekReporter")]
        public async Task<ActionResult> AssignCurrentWeekReporter([FromBody] AssignReporterDTO request)
        {
            await _teamMemberService.AssignCurrentWeekReporter(request.TeamId, request.MemberId);
            return Ok();
        }

        [HttpPost("GetMemberActiveTeams")]
        public async Task<IActionResult> GetActiveTeams([FromBody] MemberDTO request)
        {
            var activeTeams =  await _teamMemberService.GetActiveTeamsByMember(request.Id);
            if (activeTeams == null)
            {
                return NotFound();
            }
            return Ok(activeTeams);
        }

        [HttpPost("GetTeamActiveMembers")]
        public async Task<IActionResult> GetActiveMembers([FromBody] TeamMemberGetDTO request)
        {
            var activeMembers = await _teamMemberService.GetTeamActiveMembers((int)request.TeamId);
            if (activeMembers == null)
            {
                return NotFound();
            }
            return Ok(activeMembers);
        }

        [HttpPost("AutomaticAssignCurrentWeekReporter")]
        public async Task<IActionResult> CurrentWeekReporterAutomaticAssignment()
        {
            await _teamMemberService.CurrentWeekReporterAutomaticAssignment();
            return Ok();
        }

    }
}
