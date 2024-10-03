using Moq;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Application.Services;
using TeamWeeklyStatus.Application.DTOs;

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
            var mockWeeklyStatus = new WeeklyStatusDTO
            {
                Id = 1,
                WeekStartDate = new DateTime(2023, 10, 17),
                DoneThisWeek = new List<DoneThisWeekTaskDTO> { new DoneThisWeekTaskDTO { TaskDescription = "Task 1", Subtasks = new List<SubtaskDTO>() /* Populate if needed */ } },
                PlanForNextWeek = new List<string> { "Plan 1"},
                Blockers = "None",
                UpcomingPTO = new List<DateTime> { new DateTime(2023, 10, 24) },
                MemberId = 2
            };
            _mockRepository.Setup(repo => repo.GetWeeklyStatusByMemberByStartDateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                           .ReturnsAsync(mockWeeklyStatus);

            // Act
            var result = await _weeklyStatusService.GetWeeklyStatusByMemberByStartDateAsync(2, 2, new DateTime(2023, 10, 17));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockWeeklyStatus.Id, result.Id);
            Assert.Equal(mockWeeklyStatus.DoneThisWeek.Select(t => t.TaskDescription), result.DoneThisWeek.Select(t => t.TaskDescription));
            // Add more assertions here for subtasks if needed
        }


        [Fact]
        public async Task AddWeeklyStatusAsync_WithValidData_ReturnsAddedDtoWithId()
        {
            // Arrange
            var dto = new WeeklyStatusDTO
            {
                WeekStartDate = new DateTime(2023, 10, 17),
                DoneThisWeek = new List<DoneThisWeekTaskDTO>
        {
            new DoneThisWeekTaskDTO { TaskDescription = "Task 1" } // Assuming DoneThisWeekTaskDTO has a TaskDescription property
        },
                PlanForNextWeek = new List<string> { "Plan 1" }, // Assuming PlanForNextWeek is still a List<string>
                Blockers = "None",
                UpcomingPTO = new List<DateTime> { new DateTime(2023, 10, 24) },
                MemberId = 2
            };
            var mockAddedStatus = new WeeklyStatus
            {
                Id = 3,
                // Other properties need to be set up if they are used in the method
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
