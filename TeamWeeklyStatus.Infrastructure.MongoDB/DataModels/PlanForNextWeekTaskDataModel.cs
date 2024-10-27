using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    public class PlanForNextWeekTaskDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        public int Id { get; set; }
        public string TaskDescription { get; set; }

        public int WeeklyStatusId { get; set; }
        // Optionally include PlanForNextWeekTaskDataModel if embedding
        public WeeklyStatusDataModel WeeklyStatus { get; set; }

        public List<SubtaskNextWeekDataModel> Subtasks { get; set; } = new List<SubtaskNextWeekDataModel>();

        // Mapping methods
        public PlanForNextWeekTask ToDomainEntity()
        {
            return new PlanForNextWeekTask
            {
                Id = this.Id,
                TaskDescription = this.TaskDescription,
                WeeklyStatusId = this.WeeklyStatusId,
                // WeeklyStatus = this.WeeklyStatus?.ToDomainEntity(), // Fetch separately if referencing
                Subtasks = this.Subtasks?.ConvertAll(s => s.ToDomainEntity())
            };
        }

        public static PlanForNextWeekTaskDataModel FromDomainEntity(PlanForNextWeekTask task)
        {
            return new PlanForNextWeekTaskDataModel
            {
                Id = task.Id,
                TaskDescription = task.TaskDescription,
                WeeklyStatusId = task.WeeklyStatusId,
                WeeklyStatus = WeeklyStatusDataModel.FromDomainEntity(task.WeeklyStatus), // If embedding
                Subtasks = task.Subtasks?.ConvertAll(SubtaskNextWeekDataModel.FromDomainEntity)
            };
        }
    }
}
