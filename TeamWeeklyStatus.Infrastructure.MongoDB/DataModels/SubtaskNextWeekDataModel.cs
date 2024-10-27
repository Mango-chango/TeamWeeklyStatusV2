using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    public class SubtaskNextWeekDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }

        public int TaskId { get; set; }

        //public int PlanNextWeekTaskId { get; set; }
        // Optionally include SubtaskNextWeekDataModel if embedding

        //public SubtaskNextWeekDataModel Task { get; set; }

        // Mapping methods
        public SubtaskNextWeek ToDomainEntity()
        {
            return new SubtaskNextWeek
            {
                Id = this.Id,
                Description = this.Description,
                TaskId = this.TaskId,
                //Task = this.Task?.ToDomainEntity()
            };
        }

        public static SubtaskNextWeekDataModel FromDomainEntity(SubtaskNextWeek subtask)
        {
            return new SubtaskNextWeekDataModel
            {
                Id = subtask.Id,
                Description = subtask.Description,
                TaskId = subtask.TaskId,
                //Task = SubtaskNextWeekDataModel.FromDomainEntity(subtask.Task)
            };
        }
    }
}
