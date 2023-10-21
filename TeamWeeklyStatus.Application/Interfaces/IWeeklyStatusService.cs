using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IWeeklyStatusService
    {
        Task<WeeklyStatusDTO> AddWeeklyStatusAsync(WeeklyStatusDTO weeklyStatus);

        Task<WeeklyStatusDTO> UpdateWeeklyStatusAsync(WeeklyStatusDTO weeklyStatus);

        Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, DateTime startDate);

        WeeklyStatusDTO GetStatusForWeek(int teamId, DateTime weekStartDate);

        List<WeeklyStatusDTO> GetStatusesForTeamMember(int teamId, int memberId);

        List<WeeklyStatusDTO> GetStatusesForTeam(int teamId);

        List<WeeklyStatusDTO> GetStatusesForTeam(int teamId, DateTime startDate);


    }
}
