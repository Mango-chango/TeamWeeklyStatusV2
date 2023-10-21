using Microsoft.EntityFrameworkCore;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public class WeeklyStatusRepository : IWeeklyStatusRepository
    {
        private readonly TeamWeeklyStatusContext _context;

        public WeeklyStatusRepository(TeamWeeklyStatusContext context)
        {
            _context = context;
        }

        public async Task<WeeklyStatus> AddWeeklyStatusAsync(WeeklyStatus weeklyStatus)
        {
            _context.WeeklyStatuses.Add(weeklyStatus);
            await _context.SaveChangesAsync();
            return weeklyStatus;
        }

        Task<WeeklyStatus> IWeeklyStatusRepository.DeleteWeeklyStatusAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<WeeklyStatus> GetWeeklyStatusAsync(int id)
        {
            return await _context.WeeklyStatuses
                .Include(ws => ws.Member)
                .Include(ws => ws.DoneThisWeekTasks)
                .Include(ws => ws.PlanForNextWeekTasks)
                .FirstOrDefaultAsync(ws => ws.Id == id);
        }

        public async Task<WeeklyStatus> GetWeeklyStatusByMemberByStartDateAsync(
            int memberId,
            DateTime startDate
        )
        {
            return await _context.WeeklyStatuses
                .Include(ws => ws.Member)
                .Include(ws => ws.DoneThisWeekTasks)
                .Include(ws => ws.PlanForNextWeekTasks)
                .FirstOrDefaultAsync(
                    ws => ws.Member.Id == memberId && ws.WeekStartDate == startDate
                );
        }

        Task<List<WeeklyStatus>> IWeeklyStatusRepository.GetWeeklyStatusesAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<WeeklyStatus>> IWeeklyStatusRepository.GetWeeklyStatusesByTeamMemberAsync(
            int teamId,
            int memberId
        )
        {
            throw new NotImplementedException();
        }

        public async Task<WeeklyStatus> UpdateWeeklyStatusAsync(WeeklyStatus weeklyStatus)
        {
            _context.WeeklyStatuses.Update(weeklyStatus);
            await _context.SaveChangesAsync();
            return weeklyStatus;
        }
    }
}
