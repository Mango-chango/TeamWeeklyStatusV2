using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.WebApi.DTOs;

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

        [HttpGet("WithoutCurrentReporter")]
        public async Task<IActionResult> GetMembersWithoutCurrentReporter()
        {
            var members = await _teamMemberService.GetMembersWithoutCurrentReporter();
            if (members == null)
            {
                return NotFound();
            }
            return Ok(members);
        }

        [HttpPost("AssignReporter")]
        public async Task<ActionResult> AssignReporter(AssignReporterRequest request)
        {
            await _teamMemberService.AssignReporter(request.MemberId);
            return Ok();
        }

        [HttpPost("GetMemberActiveTeams")]
        public async Task<IActionResult> GetActiveTeams(MemberTeamsRequest request)
        {
            var activeTeams =  await _teamMemberService.GetActiveTeamsByMember(request.MemberId);
            if (activeTeams == null)
            {
                return NotFound();
            }
            return Ok(activeTeams);
        }

    }
}
