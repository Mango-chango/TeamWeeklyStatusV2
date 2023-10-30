using Moq;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.DTOs;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.Application.Services;

namespace TeamWeeklyStatus.Application.Tests
{
    public class WeeklyStatusServiceTests
    {
        private readonly IWeeklyStatusService _weeklyStatusService;
        private readonly Mock<IWeeklyStatusRepository> _mockRepository;

        public WeeklyStatusServiceTests()
        {
            _mockRepository = new Mock<IWeeklyStatusRepository>();
            _weeklyStatusService = new WeeklyStatusService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetWeeklyStatusByMemberByStartDateAsync_WithValidData_ReturnsCorrectDto()
        {
            // Arrange
            var mockWeeklyStatus = new WeeklyStatus
            {
                Id = 1,
                WeekStartDate = new DateTime(2023, 10, 17),
                DoneThisWeekTasks = new List<DoneThisWeekTask> { new DoneThisWeekTask { TaskDescription = "Task 1" } },
                PlanForNextWeekTasks = new List<PlanForNextWeekTask> { new PlanForNextWeekTask { TaskDescription = "Plan 1" } },
                Blockers = "None",
                UpcomingPTO = new List<DateTime> { new DateTime(2023, 10, 24) },
                MemberId = 2
            };
            _mockRepository.Setup(repo => repo.GetWeeklyStatusByMemberByStartDateAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                           .ReturnsAsync(mockWeeklyStatus);

            // Act
            var result = await _weeklyStatusService.GetWeeklyStatusByMemberByStartDateAsync(2, new DateTime(2023, 10, 17));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockWeeklyStatus.Id, result.Id);
            Assert.Equal(mockWeeklyStatus.DoneThisWeekTasks.First().TaskDescription, result.DoneThisWeek.First());
        }

        [Fact]
        public async Task AddWeeklyStatusAsync_WithValidData_ReturnsAddedDtoWithId()
        {
            // Arrange
            var dto = new WeeklyStatusDTO
            {
                WeekStartDate = new DateTime(2023, 10, 17),
                DoneThisWeek = new List<string> { "Task 1" },
                PlanForNextWeek = new List<string> { "Plan 1" },
                Blockers = "None",
                UpcomingPTO = new List<DateTime> { new DateTime(2023, 10, 24) },
                MemberId = 2
            };
            var mockAddedStatus = new WeeklyStatus
            {
                Id = 3
            };
            _mockRepository.Setup(repo => repo.AddWeeklyStatusAsync(It.IsAny<WeeklyStatus>()))
                           .ReturnsAsync(mockAddedStatus);

            // Act
            var result = await _weeklyStatusService.AddWeeklyStatusAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockAddedStatus.Id, result.Id);
        }
    }
}
