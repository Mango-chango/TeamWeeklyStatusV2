using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    public class TeamMemberDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        // Map domain properties
        public int TeamId { get; set; }
        public TeamDataModel Team { get; set; }
        public int MemberId { get; set; }
        public MemberDataModel Member { get; set; }
        public bool? IsTeamLead { get; set; }
        public bool? IsCurrentWeekReporter { get; set; }
        public DateTime? StartActiveDate { get; set; }
        public DateTime? EndActiveDate { get; set; }

        // Optional: methods to map to/from domain entity
        public TeamMember ToDomainEntity()
        {
            return new TeamMember
            {
                TeamId = this.TeamId,
                Team = this.Team?.ToDomainEntity(),
                MemberId = this.MemberId,
                Member = this.Member?.ToDomainEntity(),
                IsTeamLead = this.IsTeamLead,
                IsCurrentWeekReporter = this.IsCurrentWeekReporter,
                StartActiveDate = this.StartActiveDate,
                EndActiveDate = this.EndActiveDate,
            };
        }

        public static TeamMemberDataModel FromDomainEntity(TeamMember teamMember)
        {
            return new TeamMemberDataModel
            {
                TeamId = teamMember.TeamId,
                Team = TeamDataModel.FromDomainEntity(teamMember.Team),
                MemberId = teamMember.MemberId,
                Member = MemberDataModel.FromDomainEntity(teamMember.Member),
                IsTeamLead = teamMember.IsTeamLead,
                IsCurrentWeekReporter = teamMember.IsCurrentWeekReporter,
                StartActiveDate = teamMember.StartActiveDate,
                EndActiveDate = teamMember.EndActiveDate,
            };
        }
    }
}
