using TeamWeeklyStatus.Domain.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IWeeklyStatusService
    {
        Task<WeeklyStatusDTO> AddWeeklyStatusAsync(WeeklyStatusDTO weeklyStatus);

        Task<WeeklyStatusDTO> UpdateWeeklyStatusAsync(WeeklyStatusDTO weeklyStatus);

        Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, int teamId, DateTime startDate);

        Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> GetAllWeeklyStatusesByStartDateAsync(DateTime weekStartDate);

    }
}
