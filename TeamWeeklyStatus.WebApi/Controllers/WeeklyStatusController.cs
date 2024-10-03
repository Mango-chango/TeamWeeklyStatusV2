using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.WebApi.DTOs;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyStatusController : ControllerBase
    {
        private readonly IWeeklyStatusService _weeklyStatusService;

        public WeeklyStatusController(IWeeklyStatusService weeklyStatusService)
        {
            _weeklyStatusService = weeklyStatusService;
        }

        [HttpPost("GetByMemberIdAndStartDate", Name = "GetWeeklyStatusByMemberByStartDate")]
        public async Task<IActionResult> GetWeeklyStatusByMemberByStartDate([FromBody] WeeklyStatusGetRequest request)
        {
            var weeklyStatus = await _weeklyStatusService.GetWeeklyStatusByMemberByStartDateAsync((int)request.MemberId, (int)request.TeamId, request.WeekStartDate);
            if (weeklyStatus == null)
            {
                return NotFound();
            }
            return Ok(weeklyStatus);
        }

        [HttpPost("GetAllWeeklyStatusesByStartDate", Name = "GetAllWeeklyStatusesByStartDate")]
        public async Task<IActionResult> GetAllWeeklyStatusesByStartDate([FromBody] WeeklyStatusGetRequest request)
        {
            var weeklyStatuses = await _weeklyStatusService.GetAllWeeklyStatusesByStartDateAsync((int)request.TeamId, request.WeekStartDate);
            if (weeklyStatuses == null)
            {
                return NotFound();
            }
            return Ok(weeklyStatuses);
        }


        [HttpPost("Add", Name = "SaveWeeklyStatus")]
        public async Task<IActionResult> SaveWeeklyStatus([FromBody] WeeklyStatusPostRequest request)
        {
            var weeklyStatusDto = new Application.DTOs.WeeklyStatusDTO
            {
                MemberId = request.MemberId,
                WeekStartDate = request.WeekStartDate,
                DoneThisWeek = request.DoneThisWeek.Select(dtw => new Application.DTOs.DoneThisWeekTaskDTO
                {
                    TaskDescription = dtw.TaskDescription,
                    Subtasks = dtw.Subtasks.Select(sub => new Application.DTOs.SubtaskDTO { SubtaskDescription = sub.SubtaskDescription }).ToList()
                }).ToList(),
                PlanForNextWeek = request.PlanForNextWeek,
                Blockers = request.Blockers,
                UpcomingPTO = request.UpcomingPTO,
                TeamId = request.TeamId
            };
            var newWeeklyStatus = await _weeklyStatusService.AddWeeklyStatusAsync(weeklyStatusDto);
            return Ok(newWeeklyStatus);
        }

        [HttpPut("Edit", Name = "UpdateWeeklyStatus")]
        public async Task<IActionResult> UpdateWeeklyStatus([FromBody] WeeklyStatusPostRequest request)
        {
            var weeklyStatusDto = new Application.DTOs.WeeklyStatusDTO
            {
                Id = request.Id,
                MemberId = request.MemberId,
                WeekStartDate = request.WeekStartDate,
                DoneThisWeek = request.DoneThisWeek.Select(dtw => new Application.DTOs.DoneThisWeekTaskDTO
                {
                    TaskDescription = dtw.TaskDescription,
                    Subtasks = dtw.Subtasks.Select(sub => new Application.DTOs.SubtaskDTO { SubtaskDescription = sub.SubtaskDescription }).ToList()
                }).ToList(),
                PlanForNextWeek = request.PlanForNextWeek,
                Blockers = request.Blockers,
                UpcomingPTO = request.UpcomingPTO,
                TeamId = request.TeamId
            };
            var updatedWeeklyStatus = await _weeklyStatusService.UpdateWeeklyStatusAsync(weeklyStatusDto);
            return Ok(updatedWeeklyStatus);
        }
    }
}
