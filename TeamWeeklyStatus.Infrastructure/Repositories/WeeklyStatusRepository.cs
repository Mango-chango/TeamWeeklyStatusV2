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

        public async Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(
            int memberId,
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
                .FirstOrDefaultAsync(
                    ws =>
                        ws.Member.Id == memberId
                        && ws.WeekStartDate.Year == dateOnly.Year
                        && ws.WeekStartDate.Month == dateOnly.Month
                        && ws.WeekStartDate.Day == dateOnly.Day
                );

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
                    .Select(task => task.TaskDescription)
                    .ToList(),
                Blockers = weeklyStatus.Blockers,
                UpcomingPTO = weeklyStatus.UpcomingPTO,
                MemberId = weeklyStatus.MemberId,
            };

            return result;
        }

        public async Task<
            IEnumerable<WeeklyStatusWithMemberNameDTO>
        > GetAllWeeklyStatusesByDateAsync(DateTime weekStartDate)
        {
            var dateOnly = weekStartDate.Date;

            var weeklyStatusesForDate = await _context.WeeklyStatuses
                .Include(ws => ws.Member)
                .Include(ws => ws.DoneThisWeekTasks)
                .ThenInclude(dtw => dtw.Subtasks) // Assuming there's a Subtasks navigation property
                .Include(ws => ws.PlanForNextWeekTasks)
                .Where(ws => ws.WeekStartDate.Date == dateOnly)
                .ToListAsync();

            var result = weeklyStatusesForDate
                .Select(
                    ws =>
                        new WeeklyStatusWithMemberNameDTO
                        {
                            MemberName = ws.Member.Name,
                            WeeklyStatus = new WeeklyStatusDTO
                            {
                                Id = ws.Id,
                                WeekStartDate = ws.WeekStartDate,
                                DoneThisWeek = ws.DoneThisWeekTasks
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
                                                                SubtaskDescription = st.Description
                                                            }
                                                    )
                                                    .ToList() // Map subtasks descriptions
                                            }
                                    )
                                    .ToList(),
                                PlanForNextWeek = ws.PlanForNextWeekTasks
                                    .Select(task => task.TaskDescription)
                                    .ToList(),
                                Blockers = ws.Blockers,
                                UpcomingPTO = ws.UpcomingPTO,
                                MemberId = ws.MemberId,
                            }
                        }
                )
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

        //public async Task DeleteSubtasksAsync(IEnumerable<int> subtaskIds)
        //{
        //    var subtasksToDelete = _context.Subtasks.Where(st => subtaskIds.Contains(st.Id));
        //    _context.Subtasks.RemoveRange(subtasksToDelete);
        //    await _context.SaveChangesAsync();
        //}
    }
}
