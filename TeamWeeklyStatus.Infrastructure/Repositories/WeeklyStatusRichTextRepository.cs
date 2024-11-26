using Microsoft.EntityFrameworkCore;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces.Repositories;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public class WeeklyStatusRichTextRepository : IWeeklyStatusRichTextRepository
    {
        private readonly TeamWeeklyStatusContext _context;

        public WeeklyStatusRichTextRepository(TeamWeeklyStatusContext context)
        {
            _context = context;
        }

        public async Task<WeeklyStatusRichText> AddWeeklyStatusAsync(WeeklyStatusRichText weeklyStatus)
        {
            _context.WeeklyStatusRichTexts.Add(weeklyStatus);
            await _context.SaveChangesAsync();
            return weeklyStatus;
        }

        Task<WeeklyStatusRichText> IWeeklyStatusRichTextRepository.DeleteWeeklyStatusAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<WeeklyStatusRichText> GetWeeklyStatusByIdAsync(int id)
        {
            return await _context.WeeklyStatusRichTexts
                .Include(ws => ws.Member)
                .FirstOrDefaultAsync(ws => ws.Id == id);
        }

        public async Task<WeeklyStatusRichTextDTO> GetWeeklyStatusByMemberByStartDateAsync(
            int memberId,
            int teamId,
            DateTime startDate)
        {
            var dateOnly = startDate.Date;

            var weeklyStatus = await _context.WeeklyStatusRichTexts
                .AsNoTracking()
                .FirstOrDefaultAsync(ws =>
                    ws.MemberId == memberId &&
                    ws.TeamId == teamId &&
                    ws.WeekStartDate.Date == dateOnly);

            if (weeklyStatus == null)
            {
                return null;
            }

            var result = new WeeklyStatusRichTextDTO
            {
                Id = weeklyStatus.Id,
                MemberId = weeklyStatus.MemberId,
                TeamId = weeklyStatus.TeamId ?? 0,
                WeekStartDate = weeklyStatus.WeekStartDate,
                DoneThisWeekContent = weeklyStatus.DoneThisWeekContent ?? string.Empty,
                PlanForNextWeekContent = weeklyStatus.PlanForNextWeekContent ?? string.Empty,
                Blockers = weeklyStatus.Blockers ?? string.Empty,
                UpcomingPTO = weeklyStatus.UpcomingPTO ?? new List<DateTime>()
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
                .Where(tm => tm.TeamId == teamId && (tm.EndActiveDate == null || (tm.StartActiveDate <= DateTime.Now && tm.EndActiveDate >= DateTime.Now)))
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

        public async Task<WeeklyStatusRichText> UpdateWeeklyStatusAsync(WeeklyStatusRichText weeklyStatus)
        {
            _context.WeeklyStatusRichTexts.Update(weeklyStatus);
            await _context.SaveChangesAsync();
            return weeklyStatus;
        }

        public async Task<WeeklyStatusRichText> GetLatestWeeklyStatusAsync(int teamId, int memberId)
        {
            return await _context.WeeklyStatusRichTexts
                .AsNoTracking()
                .Where(ws => ws.TeamId == teamId && ws.MemberId == memberId)
                .OrderByDescending(ws => ws.CreatedDate)
                .FirstOrDefaultAsync();
        }
    }
}
