using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Domain.Enums;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces.Services;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyStatusController : ControllerBase
    {
        private readonly IWeeklyStatusService _weeklyStatusService;
        private readonly IReminderService _reminderService;

        public WeeklyStatusController(IWeeklyStatusService weeklyStatusService, IReminderService reminderService)
        {
            _weeklyStatusService = weeklyStatusService;
            _reminderService = reminderService;
        }

        [HttpPost("GetByMemberIdAndStartDate", Name = "GetWeeklyStatusByMemberByStartDate")]
        public async Task<IActionResult> GetWeeklyStatusByMemberByStartDate([FromBody] WeeklyStatusGetDTO request)
        {
            var weeklyStatus = await _weeklyStatusService.GetWeeklyStatusByMemberByStartDateAsync((int)request.MemberId, (int)request.TeamId, request.WeekStartDate);
            if (weeklyStatus == null)
            {
                return NotFound();
            }
            return Ok(weeklyStatus);
        }

        [HttpPost("GetAllWeeklyStatusesByStartDate", Name = "GetAllWeeklyStatusesByStartDate")]
        public async Task<IActionResult> GetAllWeeklyStatusesByStartDate([FromBody] WeeklyStatusGetDTO request)
        {
            var weeklyStatuses = await _weeklyStatusService.GetAllWeeklyStatusesByStartDateAsync((int)request.TeamId, request.WeekStartDate);
            if (weeklyStatuses == null)
            {
                return NotFound();
            }
            return Ok(weeklyStatuses);
        }


        [HttpPost("Add", Name = "SaveWeeklyStatus")]
        public async Task<IActionResult> SaveWeeklyStatus([FromBody] WeeklyStatusDTO request)
        {
            var newWeeklyStatus = await _weeklyStatusService.AddWeeklyStatusAsync(request);
            return Ok(newWeeklyStatus);
        }

        [HttpPut("Edit", Name = "UpdateWeeklyStatus")]
        public async Task<IActionResult> UpdateWeeklyStatus([FromBody] WeeklyStatusDTO request)
        {
            var updatedWeeklyStatus = await _weeklyStatusService.UpdateWeeklyStatusAsync(request);
            return Ok(updatedWeeklyStatus);
        }

        [HttpPost("SendReminders", Name = "SendReminders")]
        public async Task<IActionResult> SendReminders([FromBody] ReminderDTO request)
        {
            if (!Enum.TryParse<EventName>(request.EventName, out var eventName))
            {
                return BadRequest("Invalid event name.");
            }
            await _reminderService.SendReminderEmails(eventName);
            return Ok();
        }
    }
}
