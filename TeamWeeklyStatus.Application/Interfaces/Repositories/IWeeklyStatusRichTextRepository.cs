using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces.Repositories
{
    public interface IWeeklyStatusRichTextRepository
    {
        Task<WeeklyStatusRichText> AddWeeklyStatusAsync(WeeklyStatusRichText weeklyStatus);
        Task<WeeklyStatusRichText> UpdateWeeklyStatusAsync(WeeklyStatusRichText weeklyStatus);
        Task<WeeklyStatusRichText> DeleteWeeklyStatusAsync(int id);
        Task<WeeklyStatusRichText> GetWeeklyStatusByIdAsync(int id);
        Task<IEnumerable<WeeklyStatusRichTextWithMemberNameDTO>> GetAllWeeklyStatusesByDateAsync(int teamId, DateTime weekStartDate);
        Task<WeeklyStatusRichTextDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, int teamId, DateTime startDate);
        Task<WeeklyStatusRichText> GetLatestWeeklyStatusAsync(int memberId, int teamId);
    }
}
