using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.Repositories
{
    public class WeeklyStatusRepository : IWeeklyStatusRepository
    {
        private readonly IMongoCollection<WeeklyStatus> _weeklyStatuses;

        public WeeklyStatusRepository(IMongoDatabase database)
        {
            _weeklyStatuses = database.GetCollection<WeeklyStatus>("WeeklyStatuses");
        }

        Task IWeeklyStatusRepository.AddSubtasksAsync(IEnumerable<Subtask> subtasks)
        {
            throw new NotImplementedException();
        }

        Task IWeeklyStatusRepository.AddSubtasksNextWeekAsync(IEnumerable<SubtaskNextWeek> subtasks)
        {
            throw new NotImplementedException();
        }

        Task<WeeklyStatus> IWeeklyStatusRepository.AddWeeklyStatusAsync(WeeklyStatus weeklyStatus)
        {
            throw new NotImplementedException();
        }

        Task<WeeklyStatus> IWeeklyStatusRepository.DeleteWeeklyStatusAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> IWeeklyStatusRepository.GetAllWeeklyStatusesByDateAsync(int teamId, DateTime weekStartDate)
        {
            throw new NotImplementedException();
        }

        Task<WeeklyStatus> IWeeklyStatusRepository.GetLatestWeeklyStatusAsync(int memberId, int teamId)
        {
            throw new NotImplementedException();
        }

        Task<WeeklyStatus> IWeeklyStatusRepository.GetWeeklyStatusByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<WeeklyStatusDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, int teamId, DateTime startDate)
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

            var filter = Builders<WeeklyStatus>.Filter.And(
                Builders<WeeklyStatus>.Filter.Eq(ws => ws.MemberId, memberId),
                Builders<WeeklyStatus>.Filter.Eq(ws => ws.TeamId, teamId),
                Builders<WeeklyStatus>.Filter.Eq(ws => ws.WeekStartDate, dateOnly)
            );

            var weeklyStatus = await _weeklyStatuses.Find(filter).FirstOrDefaultAsync();

            if (weeklyStatus == null)
            {
                return null;
            }

            return new WeeklyStatusDTO
            {
                Id = weeklyStatus.Id,
                WeekStartDate = weeklyStatus.WeekStartDate,
                DoneThisWeek = weeklyStatus.DoneThisWeekTasks.Select(task => new DoneThisWeekTaskDTO
                {
                    Id = task.Id,
                    TaskDescription = task.TaskDescription,
                    Subtasks = task.Subtasks.Select(subtask => new SubtaskDTO
                    {
                        SubtaskDescription = subtask.Description
                    }).ToList()
                }).ToList(),
                PlanForNextWeek = weeklyStatus.PlanForNextWeekTasks.Select(task => new PlanForNextWeekTaskDTO
                {
                    Id = task.Id,
                    TaskDescription = task.TaskDescription,
                    Subtasks = task.Subtasks.Select(subtask => new SubtaskNextWeekDTO
                    {
                        SubtaskDescription = subtask.Description
                    }).ToList()
                }).ToList(),
                Blockers = weeklyStatus.Blockers,
                UpcomingPTO = weeklyStatus.UpcomingPTO,
                MemberId = weeklyStatus.MemberId
            };
        }

        Task IWeeklyStatusRepository.UpdateSubtasksAsync(IEnumerable<Subtask> subtasks)
        {
            throw new NotImplementedException();
        }

        Task IWeeklyStatusRepository.UpdateSubtasksNextWeekAsync(IEnumerable<SubtaskNextWeek> subtasks)
        {
            throw new NotImplementedException();
        }

        Task<WeeklyStatus> IWeeklyStatusRepository.UpdateWeeklyStatusAsync(WeeklyStatus weeklyStatus)
        {
            throw new NotImplementedException();
        }
    }
}
