using Microsoft.EntityFrameworkCore;
using TeamWeeklyStatus.Domain.DTOs;
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

        public async Task<WeeklyStatus> GetWeeklyStatusByIdAsync(int id)
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

        public async Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> GetAllWeeklyStatusesByDateAsync(DateTime weekStartDate)
        {
            // Get all team members
            var allTeamMembers = await _context.Members.ToListAsync();

            // Get all weekly statuses for the given date
            var weeklyStatusesForDate = await _context.WeeklyStatuses
                .Include(ws => ws.Member)
                .Where(ws => ws.WeekStartDate == weekStartDate)
                .ToListAsync();

            // Create a result list with member names and their weekly status if it exists
            var result = allTeamMembers.Select(member => new WeeklyStatusWithMemberNameDTO
            {
                MemberName = member.Name,
                WeeklyStatus = weeklyStatusesForDate.FirstOrDefault(ws => ws.Member.Id == member.Id)
            }).OrderBy(dto => dto.MemberName).ToList();

            return result;
        }

        public async Task<WeeklyStatus> UpdateWeeklyStatusAsync(WeeklyStatus weeklyStatus)
        {
            _context.WeeklyStatuses.Update(weeklyStatus);
            await _context.SaveChangesAsync();
            return weeklyStatus;
        }
    }
}
