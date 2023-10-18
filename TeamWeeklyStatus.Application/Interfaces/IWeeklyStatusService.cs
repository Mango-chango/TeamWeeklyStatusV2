using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IWeeklyStatusService
    {
        WeeklyStatusDTO GetStatusForWeek(int teamId, DateTime weekStartDate);
        void UpdateStatusForWeek(int memberId, WeeklyStatusDTO weeklyStatus);
    }
}
