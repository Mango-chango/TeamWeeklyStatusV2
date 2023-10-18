using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IRepository<Member> _memberRepository;

        public MemberController(IRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Member>> GetMembers()
        {
            return Ok(_memberRepository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Member> GetMember(int id)
        {
            var member = _memberRepository.GetById(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }
    }
}
