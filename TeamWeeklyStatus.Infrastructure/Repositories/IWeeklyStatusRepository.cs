using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public interface IWeeklyStatusRepository
    {
        Task<WeeklyStatus> AddWeeklyStatusAsync(WeeklyStatus weeklyStatus);
        Task<WeeklyStatus> UpdateWeeklyStatusAsync(WeeklyStatus weeklyStatus);
        Task<WeeklyStatus> DeleteWeeklyStatusAsync(int id);
        Task<WeeklyStatus> GetWeeklyStatusAsync(int id);
        Task<WeeklyStatus> GetWeeklyStatusByMemberByStartDateAsync(int memberId, DateTime startDate);
        Task<List<WeeklyStatus>> GetWeeklyStatusesAsync();
        Task<List<WeeklyStatus>> GetWeeklyStatusesByTeamMemberAsync(int teamId, int memberId);
    }
}
