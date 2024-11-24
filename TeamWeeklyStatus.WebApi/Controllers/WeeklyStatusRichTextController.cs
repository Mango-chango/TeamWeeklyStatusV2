using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Enums;
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyStatusRichTextController : ControllerBase
    {
        private readonly IWeeklyStatusRichTextService _weeklyStatusService;
        private readonly IReminderService _reminderService;
        private readonly IAIService _aiService;

        public WeeklyStatusRichTextController(IWeeklyStatusRichTextService weeklyStatusService, IReminderService reminderService, IAIService aiService)
        {
            _weeklyStatusService = weeklyStatusService;
            _reminderService = reminderService;
            _aiService = aiService;
        }

        [HttpPost("GetByMemberIdAndStartDate1", Name = "GetWeeklyStatusByMemberByStartDate1")]
        public async Task<IActionResult> GetWeeklyStatusByMemberByStartDate([FromBody] WeeklyStatusGetDTO request)
        {
            var weeklyStatus = await _weeklyStatusService.GetWeeklyStatusByMemberByStartDateAsync((int)request.MemberId, (int)request.TeamId, request.WeekStartDate);
            if (weeklyStatus == null)
            {
                return NotFound();
            }
            return Ok(weeklyStatus);
        }

        [HttpPost("GetAllWeeklyStatusesByStartDate1", Name = "GetAllWeeklyStatusesByStartDate1")]
        public async Task<IActionResult> GetAllWeeklyStatusesByStartDate([FromBody] WeeklyStatusGetDTO request)
        {
            var weeklyStatuses = await _weeklyStatusService.GetAllWeeklyStatusesByStartDateAsync((int)request.TeamId, request.WeekStartDate);
            if (weeklyStatuses == null)
            {
                return NotFound();
            }
            return Ok(weeklyStatuses);
        }


        [HttpPost("Add1", Name = "SaveWeeklyStatus1")]
        public async Task<IActionResult> SaveWeeklyStatus([FromBody] WeeklyStatusRichTextDTO request)
        {
            var newWeeklyStatus = await _weeklyStatusService.AddWeeklyStatusAsync(request);
            return Ok(newWeeklyStatus);
        }

        [HttpPut("Edit1", Name = "UpdateWeeklyStatus1")]
        public async Task<IActionResult> UpdateWeeklyStatus([FromBody] WeeklyStatusRichTextDTO request)
        {
            var updatedWeeklyStatus = await _weeklyStatusService.UpdateWeeklyStatusAsync(request);
            return Ok(updatedWeeklyStatus);
        }

        [HttpPost("SendReminders1", Name = "SendReminders1")]
        public async Task<IActionResult> SendReminders([FromBody] ReminderDTO request)
        {
            if (!Enum.TryParse<EventName>(request.EventName, out var eventName))
            {
                return BadRequest("Invalid event name.");
            }
            await _reminderService.SendReminderEmails(eventName);
            return Ok();
        }

        [HttpPost("GetAIEnhancedContent", Name = "GetAIEnhancedContent")]
        public async Task<IActionResult> GetAIEnhancedContent([FromBody] PromptDTO request)
        {
            var response = await _aiService.EnhanceTextAsync(request.Content);
            return Ok(response);
        }
    }
}
