using TeamWeeklyStatus.Domain.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public interface IWeeklyStatusRepository
    {
        Task<WeeklyStatus> AddWeeklyStatusAsync(WeeklyStatus weeklyStatus);
        Task<WeeklyStatus> UpdateWeeklyStatusAsync(WeeklyStatus weeklyStatus);
        Task<WeeklyStatus> DeleteWeeklyStatusAsync(int id);
        Task<WeeklyStatus> GetWeeklyStatusByIdAsync(int id);
        Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> GetAllWeeklyStatusesByDateAsync(DateTime weekStartDate);
        Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, DateTime startDate);

        Task AddSubtasksAsync(IEnumerable<Subtask> subtasks);
        Task UpdateSubtasksAsync(IEnumerable<Subtask> subtasks);
        //Task DeleteSubtasksAsync(IEnumerable<Subtask> subtasks);
    }
}
