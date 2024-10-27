using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    public class MemberDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        // Map domain properties
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }


        // Optional: methods to map to/from domain entity
        public Member ToDomainEntity()
        {
            return new Member
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                IsAdmin = this.IsAdmin,
            };
        }

        public static MemberDataModel FromDomainEntity(Member member)
        {
            return new MemberDataModel
            {
                // Note: MongoId is not set here; it will be assigned by MongoDB on insert
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                IsAdmin = (bool)member.IsAdmin,
            };
        }
    }
}
