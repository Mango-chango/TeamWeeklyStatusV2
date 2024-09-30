using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.DTOs;
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

        [HttpPost("GetTeamMembers")]
        public async Task<IActionResult> GetTeamMembers([FromBody] TeamMemberRequest request)
        {
            var members = await _teamMemberService.GetAllTeamMembersAsync((int)request.TeamId);
            if (members == null)
            {
                return NotFound();
            }
            return Ok(members);
        }

        [HttpPost("AddTeamMember")]
        public async Task<IActionResult> AddTeamMember([FromBody] TeamMemberPostRequest request)
        {
            var teamMemberDto = new TeamMemberDTO
            {
                TeamId = request.TeamId,
                MemberId = request.MemberId,
                IsTeamLead = request.IsTeamLead,
                IsCurrentWeekReporter = request.IsCurrentWeekReporter,
                StartActiveDate = request.StartActiveDate,
                EndActiveDate = request.EndActiveDate
            };

            var addedTeamMember = await _teamMemberService.AddTeamMemberAsync(teamMemberDto);
            return Ok(addedTeamMember);
        }

        [HttpPost("UpdateTeamMember")]
        public async Task<IActionResult> UpdateTeamMember([FromBody] TeamMemberPostRequest request)
        {
            var teamMemberDto = new TeamMemberDTO
            {
                TeamId = request.TeamId,
                MemberId = request.MemberId,
                IsTeamLead = request.IsTeamLead,
                IsCurrentWeekReporter = request.IsCurrentWeekReporter,
                StartActiveDate = request.StartActiveDate,
                EndActiveDate = request.EndActiveDate
            };

            var updatedTeamMember = await _teamMemberService.UpdateTeamMemberAsync(teamMemberDto);
            return Ok(updatedTeamMember);
        }

        [HttpPost("RemoveTeamMember")]
        public async Task<IActionResult> RemoveTeamMember([FromBody] TeamMemberRequest request)
        {
            var teamMemberDto = new TeamMemberDTO
            {
                TeamId = (int)request.TeamId,
                MemberId = (int)request.MemberId,
            };
            await _teamMemberService.DeleteTeamMemberAsync(teamMemberDto);
            return Ok();
        }

        [HttpPost("GetTeamMembersWithTeamData")]
        public async Task<IActionResult> GetTeamMembersWithTeamData(TeamMemberRequest request)
        {
            var members = await _teamMemberService.GetTeamMemberByEmailWithTeamData(request.Email);
            if (members == null)
            {
                return NotFound();
            }
            return Ok(members);
        }

        //[HttpPost("GetTeamMembersWithTeamDataById")]
        //public async Task<IActionResult> GetTeamMembersWithTeamDataById(TeamMemberRequest request)
        //{
        //    var members = await _teamMemberService.GetTeamMemberByIdWithTeamData((int)request.TeamId);
        //    if (members == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(members);
        //}

        [HttpGet("ExcludingCurrentReporter")]
        public async Task<IActionResult> GetTeamMembersExcludingCurrentReporter([FromBody] TeamMemberRequest request)
        {
            var members = await _teamMemberService.GetTeamMemberByEmailWithTeamData(request.Email);
            if (members == null)
            {
                return NotFound();
            }
            return Ok(members);
        }

        [HttpPost("AssignWeekReporter")]
        public async Task<ActionResult> AssignWeekReporter([FromBody] AssignReporterRequest request)
        {
            await _teamMemberService.AssignWeekReporter(request.TeamId, request.MemberId);
            return Ok();
        }

        [HttpPost("GetMemberActiveTeams")]
        public async Task<IActionResult> GetActiveTeams([FromBody] MemberTeamsRequest request)
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
