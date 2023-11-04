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
            // Ensure that startDate only contains the date component
            var dateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, startDate.Kind);

            return await _context.WeeklyStatuses
                .Include(ws => ws.Member)
                .Include(ws => ws.DoneThisWeekTasks)
                .Include(ws => ws.PlanForNextWeekTasks)
                .FirstOrDefaultAsync(
                    ws => ws.Member.Id == memberId &&
                          ws.WeekStartDate.Year == dateOnly.Year &&
                          ws.WeekStartDate.Month == dateOnly.Month &&
                          ws.WeekStartDate.Day == dateOnly.Day
                );
        }


        public async Task<
            IEnumerable<WeeklyStatusWithMemberNameDTO>
        > GetAllWeeklyStatusesByDateAsync(DateTime weekStartDate)
        {
            // Get all team members
            var allTeamMembers = await _context.Members.ToListAsync();

            var dateOnly = new DateTime(weekStartDate.Year, weekStartDate.Month, weekStartDate.Day, 0, 0, 0, weekStartDate.Kind);

            // Get all weekly statuses for the given date
            var weeklyStatusesForDate = await _context.WeeklyStatuses
                .Include(ws => ws.Member)
                .Include(ws => ws.DoneThisWeekTasks)
                .Include(ws => ws.PlanForNextWeekTasks)
                .Where(ws => ws.WeekStartDate.Year == dateOnly.Year &&
                          ws.WeekStartDate.Month == dateOnly.Month &&
                          ws.WeekStartDate.Day == dateOnly.Day)
                .ToListAsync();

            // Create a result list with member names and their weekly status if it exists
            var result = allTeamMembers
                .Select(member =>
                {
                    var matchingStatus = weeklyStatusesForDate.FirstOrDefault(
                        ws => ws.Member.Id == member.Id
                    );
                    return new WeeklyStatusWithMemberNameDTO
                    {
                        MemberName = member.Name,
                        WeeklyStatus =
                            matchingStatus != null
                                ? new WeeklyStatusDTO
                                {
                                    Id = matchingStatus.Id,
                                    WeekStartDate = matchingStatus.WeekStartDate,
                                    DoneThisWeek = matchingStatus.DoneThisWeekTasks
                                        .Select(task => task.TaskDescription)
                                        .ToList(),
                                    PlanForNextWeek = matchingStatus.PlanForNextWeekTasks
                                        .Select(task => task.TaskDescription)
                                        .ToList(),
                                    Blockers = matchingStatus.Blockers,
                                    UpcomingPTO = matchingStatus.UpcomingPTO,
                                    MemberId = matchingStatus.MemberId,
                                }
                                : null
                    };
                })
                .OrderBy(dto => dto.MemberName)
                .ToList();

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
