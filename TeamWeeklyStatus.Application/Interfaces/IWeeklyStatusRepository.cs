using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IWeeklyStatusRepository
    {
        Task<WeeklyStatus> AddWeeklyStatusAsync(WeeklyStatus weeklyStatus);
        Task<WeeklyStatus> UpdateWeeklyStatusAsync(WeeklyStatus weeklyStatus);
        Task<WeeklyStatus> DeleteWeeklyStatusAsync(int id);
        Task<WeeklyStatus> GetWeeklyStatusByIdAsync(int id);
        Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> GetAllWeeklyStatusesByDateAsync(int teamId, DateTime weekStartDate);
        Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, int teamId, DateTime startDate);

        Task AddSubtasksAsync(IEnumerable<Subtask> subtasks);
        Task UpdateSubtasksAsync(IEnumerable<Subtask> subtasks);
        //Task DeleteSubtasksAsync(IEnumerable<Subtask> subtasks);
    }
}
