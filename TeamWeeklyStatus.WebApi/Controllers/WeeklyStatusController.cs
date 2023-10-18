using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyStatusController : ControllerBase
    {
        private readonly IRepository<WeeklyStatus> _statusRepository;

        public WeeklyStatusController(IRepository<WeeklyStatus> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeeklyStatus>> GetStatuses()
        {
            return Ok(_statusRepository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<WeeklyStatus> GetStatus(int id)
        {
            var status = _statusRepository.GetById(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }
    }
}
