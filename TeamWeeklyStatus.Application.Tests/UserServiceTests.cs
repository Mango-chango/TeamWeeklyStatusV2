using Moq;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.Services;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Tests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;
        private readonly Mock<ITeamMemberRepository> _mockTeamMemberRepository;
        private readonly Mock<IMemberRepository> _mockMemberRepository;

        public UserServiceTests()
        {
            _mockTeamMemberRepository = new Mock<ITeamMemberRepository>();
            _mockMemberRepository = new Mock<IMemberRepository>();
            _userService = new UserService(_mockTeamMemberRepository.Object, _mockMemberRepository.Object);
        }

        [Fact]
        public async Task ValidateUserWithInvalidEmailReturnsInvalidValidationResult()
        {
            // Arrange
            _mockMemberRepository.Setup(repo => repo.GetMemberByEmailAsync(It.IsAny<string>()))
                           .ReturnsAsync((Member)null);

            // Act
            var result = await _userService.ValidateUser("invalid.email@changos.com");

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Invalid email address.", result.ErrorMessage);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task ValidateUserIsTeamLead(bool? isTeamLead, bool? isCurrentWeekReporter)
        {
            // Arrange
            var mockMember = new TeamMember
            {
                IsTeamLead = isTeamLead,
                IsCurrentWeekReporter = isCurrentWeekReporter,
                Member = new Member { Name = "Guicho el Monkey" },
                Team = new Team { Name = "Team Coolest Changos" }
            };

            _mockMemberRepository.Setup(repo => repo.GetMemberByEmailAsync(It.IsAny<string>()))
                           .ReturnsAsync(new Member { Name = "Guicho el Monkey" });

            _mockTeamMemberRepository.Setup(repo => repo.GetTeamMemberAsync(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(mockMember);

            // Act
            var result = await _userService.ValidateUser("guicho@changos.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(isTeamLead, mockMember.IsTeamLead);
            Assert.Equal(isCurrentWeekReporter, mockMember.IsCurrentWeekReporter);
            Assert.Equal("Team Coolest Changos", mockMember.Team.Name);
            Assert.Equal("Guicho el Monkey", mockMember.Member.Name);
        }
    }
}
