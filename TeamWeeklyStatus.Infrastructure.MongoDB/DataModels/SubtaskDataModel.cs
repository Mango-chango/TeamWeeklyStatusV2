using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    public class SubtaskDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }

        public int TaskId { get; set; }

        //public int DoneThisWeekTaskId { get; set; }
        // Optionally include DoneThisWeekTaskDataModel if embedding
        
        //public DoneThisWeekTaskDataModel Task { get; set; }

        // Mapping methods
        public Subtask ToDomainEntity()
        {
            return new Subtask
            {
                Id = this.Id,
                Description = this.Description,
                TaskId = this.TaskId,
                //Task = this.Task?.ToDomainEntity()
            };
        }

        public static SubtaskDataModel FromDomainEntity(Subtask subtask)
        {
            return new SubtaskDataModel
            {
                Id = subtask.Id,
                Description = subtask.Description,
                TaskId = subtask.TaskId,
                //Task = DoneThisWeekTaskDataModel.FromDomainEntity(subtask.Task)
            };
        }
    }
}
