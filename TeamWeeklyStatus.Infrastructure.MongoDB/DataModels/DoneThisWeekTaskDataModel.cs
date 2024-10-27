// Infrastructure.MongoDB/DataModels/DoneThisWeekTaskDataModel.cs

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    public class DoneThisWeekTaskDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        public int Id { get; set; }
        public string TaskDescription { get; set; }

        public int WeeklyStatusId { get; set; }
        // Optionally include WeeklyStatusDataModel if embedding
        public WeeklyStatusDataModel WeeklyStatus { get; set; }

        public List<SubtaskDataModel> Subtasks { get; set; } = new List<SubtaskDataModel>();

        // Mapping methods
        public DoneThisWeekTask ToDomainEntity()
        {
            return new DoneThisWeekTask
            {
                Id = this.Id,
                TaskDescription = this.TaskDescription,
                WeeklyStatusId = this.WeeklyStatusId,
                // WeeklyStatus = this.WeeklyStatus?.ToDomainEntity(), // Fetch separately if referencing
                Subtasks = this.Subtasks?.ConvertAll(s => s.ToDomainEntity())
            };
        }

        public static DoneThisWeekTaskDataModel FromDomainEntity(DoneThisWeekTask task)
        {
            return new DoneThisWeekTaskDataModel
            {
                Id = task.Id,
                TaskDescription = task.TaskDescription,
                WeeklyStatusId = task.WeeklyStatusId,
                WeeklyStatus = WeeklyStatusDataModel.FromDomainEntity(task.WeeklyStatus), // If embedding
                Subtasks = task.Subtasks?.ConvertAll(SubtaskDataModel.FromDomainEntity)
            };
        }
    }
}
