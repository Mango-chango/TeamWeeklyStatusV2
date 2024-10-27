using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    public class WeeklyStatusDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string Blockers { get; set; }
        public List<DateTime> UpcomingPTO { get; set; }

        public int MemberId { get; set; }
        // Optionally, include MemberDataModel if embedding
        // public MemberDataModel Member { get; set; }

        public int? TeamId { get; set; }
        // Optionally, include TeamDataModel if embedding
        // public TeamDataModel Team { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<DoneThisWeekTaskDataModel> DoneThisWeekTasks { get; set; }
        public List<PlanForNextWeekTaskDataModel> PlanForNextWeekTasks { get; set; }

        // Mapping methods...

        public WeeklyStatus ToDomainEntity()
        {
            return new WeeklyStatus
            {
                Id = this.Id,
                WeekStartDate = this.WeekStartDate,
                Blockers = this.Blockers,
                UpcomingPTO = this.UpcomingPTO,
                MemberId = this.MemberId,
                // Member = this.Member?.ToDomainEntity(), // If embedding Member
                TeamId = this.TeamId,
                // Team = this.Team?.ToDomainEntity(), // If embedding Team
                CreatedDate = this.CreatedDate,
                DoneThisWeekTasks = this.DoneThisWeekTasks?.ConvertAll(dt => dt.ToDomainEntity()),
                PlanForNextWeekTasks = this.PlanForNextWeekTasks?.ConvertAll(pt => pt.ToDomainEntity())
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
                // Member = MemberDataModel.FromDomainEntity(weeklyStatus.Member), // If embedding Member
                TeamId = weeklyStatus.TeamId,
                // Team = TeamDataModel.FromDomainEntity(weeklyStatus.Team), // If embedding Team
                CreatedDate = weeklyStatus.CreatedDate,
                DoneThisWeekTasks = weeklyStatus.DoneThisWeekTasks?.ConvertAll(DoneThisWeekTaskDataModel.FromDomainEntity),
                PlanForNextWeekTasks = weeklyStatus.PlanForNextWeekTasks?.ConvertAll(PlanForNextWeekTaskDataModel.FromDomainEntity)
            };
        }
    }
}
