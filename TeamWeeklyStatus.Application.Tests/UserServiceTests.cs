using Moq;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.Application.Services;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Tests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;
        private readonly Mock<ITeamMemberRepository> _mockRepository;

        public UserServiceTests()
        {
            _mockRepository = new Mock<ITeamMemberRepository>();
            _userService = new UserService(_mockRepository.Object);
        }

        [Fact]
        public async Task ValidateUser_WithInvalidEmail_ReturnsInvalidValidationResult()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByEmailWithTeamData(It.IsAny<string>()))
                           .ReturnsAsync((TeamMember)null);

            // Act
            var result = await _userService.ValidateUser("invalid.email@changos.com");

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Invalid email address.", result.ErrorMessage);
        }

        [Theory]
        [InlineData(true, null, "TeamLead")]
        [InlineData(null, true, "CurrentWeekReporter")]
        [InlineData(null, null, "Normal")]
        public async Task ValidateUser_WithValidEmail_ReturnsCorrectRole(bool? isTeamLead, bool? isCurrentWeekReporter, string expectedRole)
        {
            // Arrange
            var mockMember = new TeamMember
            {
                IsTeamLead = isTeamLead,
                IsCurrentWeekReporter = isCurrentWeekReporter,
                Member = new Member { Name = "Guicho el Monkey" },
                Team = new Team { Name = "Team Coolest Changos" }
            };

            _mockRepository.Setup(repo => repo.GetByEmailWithTeamData(It.IsAny<string>()))
                           .ReturnsAsync(mockMember);

            // Act
            var result = await _userService.ValidateUser("macaco@changos.com");

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(expectedRole, result.Role);
            Assert.Equal("Team Coolest Changos", result.TeamName);
            Assert.Equal("Guicho el Monkey", result.MemberName);
        }

    }
}
