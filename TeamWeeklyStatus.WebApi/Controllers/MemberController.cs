using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.Services;

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
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpGet("GetAll", Name = "GetAllMembers")]
        public async Task<IActionResult> GetAllMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            return Ok(members);
        }

        [HttpPost("Add", Name = "AddMember")]
        public async Task<IActionResult> CreateMember([FromBody] MemberDTO member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newMember = await _memberService.AddMemberAsync(member);
            return Ok(newMember);
        }

        [HttpPut("Update", Name ="UpdateMember")]
        public async Task<IActionResult> UpdateMember([FromBody] MemberDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMember = await _memberService.GetMemberByIdAsync(request.Id);
            if (existingMember == null)
            {
                return NotFound();
            }

            existingMember.Name = request.Name;
            existingMember.Email = request.Email;
            existingMember.IsAdmin = request.IsAdmin ?? false;

            var updatedMember = await _memberService.UpdateMemberAsync(existingMember);
            return Ok(updatedMember);
        }

        [HttpDelete("Delete", Name="DeleteMember")]
        public async Task<IActionResult> DeleteMember([FromBody] MemberDTO request)
        {
            var existingMember = await _memberService.GetMemberByIdAsync(request.Id);
            if (existingMember == null)
            {
                return NotFound();
            }

            var deletedMember = await _memberService.DeleteMemberAsync(existingMember);
            return Ok(deletedMember);
        }
    }
}
