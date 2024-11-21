using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.MongoDB.DataModels;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.Repositories
{
    public class WeeklyStatusRepository : IWeeklyStatusRepository
    {
        private readonly IMongoCollection<WeeklyStatusDataModel> _weeklyStatuses;

        public WeeklyStatusRepository(IMongoDatabase database)
        {
            _weeklyStatuses = database.GetCollection<WeeklyStatusDataModel>("WeeklyStatuses");
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

            var filter = Builders<WeeklyStatusDataModel>.Filter.And(
                Builders<WeeklyStatusDataModel>.Filter.Eq(ws => ws.MemberId, memberId),
                Builders<WeeklyStatusDataModel>.Filter.Eq(ws => ws.TeamId, teamId)
            );

            var weeklyStatusDataModel = await _weeklyStatuses.Find(filter).FirstOrDefaultAsync();

            if (weeklyStatusDataModel == null)
            {
                return null;
            }

            var weeklyStatus = weeklyStatusDataModel.ToDomainEntity();

            return new WeeklyStatusDTO
            {
                Id = weeklyStatus.Id,
                WeekStartDate = weeklyStatus.WeekStartDate,
                DoneThisWeek = weeklyStatus.DoneThisWeekTasks?.Select(task => new DoneThisWeekTaskDTO
                {
                    Id = task.Id,
                    TaskDescription = task.TaskDescription
                }).ToList() ?? new List<DoneThisWeekTaskDTO>(),
                PlanForNextWeek = weeklyStatus.PlanForNextWeekTasks?.Select(task => new PlanForNextWeekTaskDTO
                {
                    Id = task.Id,
                    TaskDescription = task.TaskDescription
                }).ToList() ?? new List<PlanForNextWeekTaskDTO>(),
                Blockers = weeklyStatus.Blockers,
                UpcomingPTO = weeklyStatus.UpcomingPTO ?? new List<DateTime>(),
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
