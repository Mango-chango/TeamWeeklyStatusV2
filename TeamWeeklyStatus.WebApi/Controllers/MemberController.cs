using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("{id}")]
        public ActionResult<MemberDTO> GetMember(int id)
        {
            var member = _memberService.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Team>> GetTeams(int memberId)
        {
            return Ok(_memberService.GetTeams(memberId));
        }
    }
}
