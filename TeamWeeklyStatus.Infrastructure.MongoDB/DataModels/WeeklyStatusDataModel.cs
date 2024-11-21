using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    [BsonIgnoreExtraElements]
    public class WeeklyStatusDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        [BsonElement("Id")]
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string Blockers { get; set; }
        public List<DateTime> UpcomingPTO { get; set; }

        public int MemberId { get; set; }

        public int? TeamId { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<DoneThisWeekTaskDataModel> DoneThisWeekTasks { get; set; }
        public List<PlanForNextWeekTaskDataModel> PlanForNextWeekTasks { get; set; }

        public WeeklyStatus ToDomainEntity()
        {
            return new WeeklyStatus
            {
                Id = this.Id,
                WeekStartDate = this.WeekStartDate,
                Blockers = this.Blockers,
                UpcomingPTO = this.UpcomingPTO ?? new List<DateTime>(),
                MemberId = this.MemberId,
                TeamId = this.TeamId,
                CreatedDate = this.CreatedDate,
                DoneThisWeekTasks = this.DoneThisWeekTasks?.Select(dt => dt.ToDomainEntity()).ToList() ?? new List<DoneThisWeekTask>(),
                PlanForNextWeekTasks = this.PlanForNextWeekTasks?.Select(pt => pt.ToDomainEntity()).ToList() ?? new List<PlanForNextWeekTask>()
            };
        }

        public static WeeklyStatusDataModel FromDomainEntity(WeeklyStatus weeklyStatus)
        {
            return new WeeklyStatusDataModel
            {
                Id = weeklyStatus.Id,
                WeekStartDate = weeklyStatus.WeekStartDate,
                Blockers = weeklyStatus.Blockers,
                UpcomingPTO = weeklyStatus.UpcomingPTO,
                MemberId = weeklyStatus.MemberId,
                TeamId = weeklyStatus.TeamId,
                CreatedDate = weeklyStatus.CreatedDate,
                DoneThisWeekTasks = weeklyStatus.DoneThisWeekTasks?.ConvertAll(DoneThisWeekTaskDataModel.FromDomainEntity),
                PlanForNextWeekTasks = weeklyStatus.PlanForNextWeekTasks?.ConvertAll(PlanForNextWeekTaskDataModel.FromDomainEntity)
            };
        }
    }
}
