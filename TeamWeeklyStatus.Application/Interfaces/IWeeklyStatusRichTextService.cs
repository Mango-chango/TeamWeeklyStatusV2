using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IWeeklyStatusRichTextService
    {
        Task<WeeklyStatusRichTextDTO> AddWeeklyStatusAsync(WeeklyStatusRichTextDTO weeklyStatus);

        Task<WeeklyStatusRichTextDTO> UpdateWeeklyStatusAsync(WeeklyStatusRichTextDTO weeklyStatus);

        Task<WeeklyStatusRichTextDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, int teamId, DateTime startDate);

        Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> GetAllWeeklyStatusesByStartDateAsync(int teamId, DateTime weekStartDate);

    }
}
