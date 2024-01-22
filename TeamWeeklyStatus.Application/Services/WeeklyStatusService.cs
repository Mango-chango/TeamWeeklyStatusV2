using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.DTOs;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamWeeklyStatus.Application.Services
{
    public class WeeklyStatusService : IWeeklyStatusService
    {
        private readonly IWeeklyStatusRepository _repository;

        public WeeklyStatusService(IWeeklyStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, DateTime startDate)
        {
            var utcStartDate = startDate.ToUniversalTime();
            var weeklyStatus = await _repository.GetWeeklyStatusByMemberByStartDateAsync(memberId, utcStartDate);

            if (weeklyStatus == null)
            {
                return null;
            }

            // Mapping DoneThisWeekTasks and their subtasks
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

        public async Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> GetAllWeeklyStatusesByStartDateAsync(DateTime weekStartDate) =>
            await _repository.GetAllWeeklyStatusesByDateAsync(weekStartDate);


        public async Task<WeeklyStatusDTO> AddWeeklyStatusAsync(WeeklyStatusDTO weeklyStatusDto)
        {
            var weeklyStatus = new WeeklyStatus
            {
                WeekStartDate = weeklyStatusDto.WeekStartDate,
                DoneThisWeekTasks = weeklyStatusDto.DoneThisWeek.Select(dtw => new DoneThisWeekTask
                {
                    TaskDescription = dtw.TaskDescription,
                    // Assuming there is a Subtasks property which is a collection of subtask entities
                    Subtasks = dtw.Subtasks.Select(st => new Subtask
                    {
                        Description = st.SubtaskDescription,
                    }).ToList()
                }).ToList(),
                PlanForNextWeekTasks = weeklyStatusDto.PlanForNextWeek.Select(desc => new PlanForNextWeekTask { TaskDescription = desc }).ToList(),
                Blockers = weeklyStatusDto.Blockers,
                UpcomingPTO = weeklyStatusDto.UpcomingPTO,
                MemberId = weeklyStatusDto.MemberId,
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
                throw new Exception("Weekly status not found");
            }

            existingStatus.WeekStartDate = weeklyStatusDto.WeekStartDate;
            existingStatus.Blockers = weeklyStatusDto.Blockers;
            existingStatus.UpcomingPTO = weeklyStatusDto.UpcomingPTO;

            // This can get complicated: adding, updating, and deleting individual tasks.
            // Simplified version: Replace all tasks with new ones. 
            existingStatus.DoneThisWeekTasks = weeklyStatusDto.DoneThisWeek.Select(dtw => new DoneThisWeekTask
            {
                TaskDescription = dtw.TaskDescription,
                // Similar subtask update logic as for AddWeeklyStatusAsync
                Subtasks = dtw.Subtasks.Select(st => new Subtask
                {
                    Description = st.SubtaskDescription
                }).ToList()
            }).ToList(); 
            
            existingStatus.PlanForNextWeekTasks = weeklyStatusDto.PlanForNextWeek.Select(desc => new PlanForNextWeekTask { TaskDescription = desc }).ToList();

            var updatedStatus = await _repository.UpdateWeeklyStatusAsync(existingStatus);

            return weeklyStatusDto;
        }
    }
}
