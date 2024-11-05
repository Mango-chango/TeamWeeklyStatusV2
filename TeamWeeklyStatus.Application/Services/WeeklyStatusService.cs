using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Exceptions;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Services
{
    public class WeeklyStatusService : IWeeklyStatusService
    {
        private readonly IWeeklyStatusRepository _repository;

        public WeeklyStatusService(IWeeklyStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, int teamId, DateTime startDate)
        {
            var utcStartDate = startDate.ToUniversalTime();
            var weeklyStatus = await _repository.GetWeeklyStatusByMemberByStartDateAsync(memberId, teamId, utcStartDate);

            if (weeklyStatus == null)
            {
                throw new WeeklyStatusNotFoundException(memberId, teamId, startDate);
            }

            var weeklyStatusDto = new WeeklyStatusDTO
            {
                Id = weeklyStatus.Id,
                WeekStartDate = weeklyStatus.WeekStartDate,
                DoneThisWeek = weeklyStatus.DoneThisWeek.Select(task => new DoneThisWeekTaskDTO
                {
                    TaskDescription = task.TaskDescription,
                    Subtasks = task.Subtasks.Select(subtask => new SubtaskDTO { SubtaskDescription = subtask.SubtaskDescription }).ToList()
                }).ToList(),
                PlanForNextWeek = weeklyStatus.PlanForNextWeek.Select(task => task).ToList(),
                Blockers = weeklyStatus.Blockers,
                UpcomingPTO = weeklyStatus.UpcomingPTO,
                MemberId = weeklyStatus.MemberId,
            };

            return weeklyStatusDto;
        }

        public async Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> GetAllWeeklyStatusesByStartDateAsync(int teamId, DateTime weekStartDate)
        {
            var teamWeeklyStatuses = await _repository.GetAllWeeklyStatusesByDateAsync(teamId, weekStartDate);
            if (teamWeeklyStatuses == null)
            {
                throw new WeeklyStatusNotFoundException(teamId, weekStartDate);
            }
            return teamWeeklyStatuses;
        }

        public async Task<WeeklyStatusDTO> AddWeeklyStatusAsync(WeeklyStatusDTO weeklyStatusDto)
        {
            var weeklyStatus = new WeeklyStatus
            {
                WeekStartDate = weeklyStatusDto.WeekStartDate,
                DoneThisWeekTasks = weeklyStatusDto.DoneThisWeek.Select(dtw => new DoneThisWeekTask
                {
                    TaskDescription = dtw.TaskDescription,
                    Subtasks = dtw.Subtasks.Select(st => new Subtask
                    {
                        Description = st.SubtaskDescription,
                    }).ToList()
                }).ToList(),
                PlanForNextWeekTasks = weeklyStatusDto.PlanForNextWeek.Select(pfnw => new PlanForNextWeekTask 
                { 
                    TaskDescription = pfnw.TaskDescription,
                    Subtasks = pfnw.Subtasks.Select(st => new SubtaskNextWeek
                    {
                        Description = st.SubtaskDescription,
                    }).ToList()
                }).ToList(),
                Blockers = weeklyStatusDto.Blockers,
                UpcomingPTO = weeklyStatusDto.UpcomingPTO,
                MemberId = weeklyStatusDto.MemberId,
                TeamId = weeklyStatusDto.TeamId,
                CreatedDate = DateTime.UtcNow,
            };

            var addedStatus = await _repository.AddWeeklyStatusAsync(weeklyStatus);

            weeklyStatusDto.Id = addedStatus.Id;
            return weeklyStatusDto;
        }

        public async Task<WeeklyStatusDTO> UpdateWeeklyStatusAsync(WeeklyStatusDTO weeklyStatusDto)
        {
            var existingStatus = await _repository.GetWeeklyStatusByIdAsync((int)weeklyStatusDto.Id);

            if (existingStatus == null)
            {
                throw new WeeklyStatusNotFoundException(weeklyStatusDto.Id);
            }

            existingStatus.WeekStartDate = weeklyStatusDto.WeekStartDate;
            existingStatus.Blockers = weeklyStatusDto.Blockers;
            existingStatus.UpcomingPTO = weeklyStatusDto.UpcomingPTO;

            // This can get complicated: adding, updating, and deleting individual tasks.
            // Simplified version: Replace all tasks with new ones. 
            existingStatus.DoneThisWeekTasks = weeklyStatusDto.DoneThisWeek.Select(dtw => new DoneThisWeekTask
            {
                TaskDescription = dtw.TaskDescription,
                Subtasks = dtw.Subtasks.Select(st => new Subtask
                {
                    Description = st.SubtaskDescription
                }).ToList()
            }).ToList();

            existingStatus.PlanForNextWeekTasks = weeklyStatusDto.PlanForNextWeek.Select(pfnw => new PlanForNextWeekTask
            {
                TaskDescription = pfnw.TaskDescription,
                Subtasks = pfnw.Subtasks.Select(st => new SubtaskNextWeek
                {
                    Description = st.SubtaskDescription
                }).ToList()
            }).ToList();

            existingStatus.CreatedDate = DateTime.UtcNow;

            var updatedStatus = await _repository.UpdateWeeklyStatusAsync(existingStatus);

            return weeklyStatusDto;
        }
    }
}
