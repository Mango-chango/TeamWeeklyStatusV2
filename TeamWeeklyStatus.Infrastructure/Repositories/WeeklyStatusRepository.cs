using Microsoft.EntityFrameworkCore;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
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

        public async Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(
            int memberId,
            int teamId,
            DateTime startDate
        )
        {
            // Ensure that startDate only contains the date component
            var dateOnly = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                0,
                0,
                0,
                startDate.Kind
            );

            var weeklyStatus = await _context.WeeklyStatuses
                .Include(ws => ws.Member)
                .Include(ws => ws.DoneThisWeekTasks)
                .ThenInclude(dtw => dtw.Subtasks)
                .Include(ws => ws.PlanForNextWeekTasks)
                .ThenInclude(pfnwt => pfnwt.Subtasks)
                .FirstOrDefaultAsync(
                    ws =>
                        ws.Member.Id == memberId
                        && ws.Team.Id == teamId
                        && ws.WeekStartDate.Year == dateOnly.Year
                        && ws.WeekStartDate.Month == dateOnly.Month
                        && ws.WeekStartDate.Day == dateOnly.Day
                );

            if (weeklyStatus == null)
            {
                return null;
            }

            var result = new WeeklyStatusDTO
            {
                Id = weeklyStatus.Id,
                WeekStartDate = weeklyStatus.WeekStartDate,
                DoneThisWeek = weeklyStatus.DoneThisWeekTasks
                    .Select(
                        dtw =>
                            new DoneThisWeekTaskDTO
                            {
                                TaskDescription = dtw.TaskDescription,
                                Subtasks = dtw.Subtasks
                                    .Select(
                                        st => new SubtaskDTO { SubtaskDescription = st.Description }
                                    )
                                    .ToList() // Map subtasks descriptions
                            }
                    )
                    .ToList(),
                PlanForNextWeek = weeklyStatus.PlanForNextWeekTasks
                    .Select(
                        pfnw =>
                            new PlanForNextWeekTaskDTO
                            {
                                TaskDescription = pfnw.TaskDescription,
                                Subtasks = pfnw.Subtasks
                                    .Select(
                                        st => new SubtaskNextWeekDTO { SubtaskDescription = st.Description }
                                    )
                                    .ToList() // Map subtasks descriptions
                            })
                    .ToList(),
                Blockers = weeklyStatus.Blockers,
                UpcomingPTO = weeklyStatus.UpcomingPTO,
                MemberId = weeklyStatus.MemberId,
            };

            return result;
        }

        public async Task<
            IEnumerable<WeeklyStatusWithMemberNameDTO>
        > GetAllWeeklyStatusesByDateAsync(int teamId, DateTime weekStartDate)
        {
            var dateOnly = weekStartDate.Date;

            // Get all team members
            var allTeamMembers = await _context.TeamMembers
                .Include(tm => tm.Member)
                .Where(tm => tm.TeamId == teamId)
                .ToListAsync();

            // Get existing weekly statuses for the date
            var weeklyStatusesForDate = await _context.WeeklyStatuses
                .Include(ws => ws.Member)
                .Include(ws => ws.DoneThisWeekTasks)
                .ThenInclude(dtw => dtw.Subtasks)
                .Include(ws => ws.PlanForNextWeekTasks)
                .ThenInclude(pfnwt => pfnwt.Subtasks)
                .Where(ws => ws.TeamId == teamId && ws.WeekStartDate.Date == dateOnly)
                .ToListAsync();

            // Combine existing statuses with missing members
            var result = allTeamMembers
                .Select(member =>
                {
                    var existingStatus = weeklyStatusesForDate.FirstOrDefault(
                        ws => ws.MemberId == member.MemberId
                    );
                    return new WeeklyStatusWithMemberNameDTO
                    {
                        MemberName = member.Member.Name,
                        WeeklyStatus =
                            existingStatus != null
                                ? new WeeklyStatusDTO
                                {
                                    Id = existingStatus.Id,
                                    WeekStartDate = existingStatus.WeekStartDate,
                                    DoneThisWeek = existingStatus.DoneThisWeekTasks
                                        .Select(
                                            dtw =>
                                                new DoneThisWeekTaskDTO
                                                {
                                                    TaskDescription = dtw.TaskDescription,
                                                    Subtasks = dtw.Subtasks
                                                        .Select(
                                                            st =>
                                                                new SubtaskDTO
                                                                {
                                                                    SubtaskDescription =
                                                                        st.Description
                                                                }
                                                        )
                                                        .ToList()
                                                }
                                        )
                                        .ToList(),
                                    PlanForNextWeek = existingStatus.PlanForNextWeekTasks
                                    .Select(
                                            pfnw =>
                                                new PlanForNextWeekTaskDTO
                                                {
                                                    TaskDescription = pfnw.TaskDescription,
                                                    Subtasks = pfnw.Subtasks
                                                        .Select(
                                                            st =>
                                                                new SubtaskNextWeekDTO
                                                                {
                                                                    SubtaskDescription =
                                                                        st.Description
                                                                }
                                                        )
                                                        .ToList()
                                                }
                                        )
                                        .ToList(),
                                    Blockers = existingStatus.Blockers,
                                    UpcomingPTO = existingStatus.UpcomingPTO,
                                    MemberId = existingStatus?.MemberId ?? 0, // Set 0 for missing reports
                                }
                                : null, // Set null for missing reports
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

        public async Task AddSubtasksAsync(IEnumerable<Subtask> subtasks)
        {
            await _context.Subtasks.AddRangeAsync(subtasks);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubtasksAsync(IEnumerable<Subtask> subtasks)
        {
            foreach (var subtask in subtasks)
            {
                var existingSubtask = await _context.Subtasks.FindAsync(subtask.Id);
                if (existingSubtask != null)
                {
                    // Update properties
                    existingSubtask.Description = subtask.Description;
                    // ... update other properties as necessary
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddSubtasksNextWeekAsync(IEnumerable<SubtaskNextWeek> subtasks)
        {
            await _context.SubtasksNextWeek.AddRangeAsync(subtasks);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubtasksNextWeekAsync(IEnumerable<SubtaskNextWeek> subtasks)
        {
            foreach (var subtask in subtasks)
            {
                var existingSubtask = await _context.SubtasksNextWeek.FindAsync(subtask.Id);
                if (existingSubtask != null)
                {
                    // Update properties
                    existingSubtask.Description = subtask.Description;
                    // ... update other properties as necessary
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<WeeklyStatus> GetLatestWeeklyStatusAsync(int teamId, int memberId)
        {
            return await _context.WeeklyStatuses
                .AsNoTracking()
                .Where(ws => ws.TeamId == teamId && ws.MemberId == memberId)
                .OrderByDescending(ws => ws.CreatedDate)
                .FirstOrDefaultAsync();
        }
    }
}
