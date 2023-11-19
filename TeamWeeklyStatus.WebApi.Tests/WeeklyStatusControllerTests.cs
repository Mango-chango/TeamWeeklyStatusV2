using Moq;
using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.WebApi.Controllers;
using TeamWeeklyStatus.Domain.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.WebApi.DTOs;

namespace TeamWeeklyStatus.WebApi.Tests
{
    public class WeeklyStatusControllerTests
    {
        private readonly WeeklyStatusController _controller;
        private readonly Mock<IWeeklyStatusService> _mockService;

        public WeeklyStatusControllerTests()
        {
            _mockService = new Mock<IWeeklyStatusService>();
            _controller = new WeeklyStatusController(_mockService.Object);
        }

        [Fact]
        public async Task GetWeeklyStatusByMemberByStartDate_WithExistingData_ReturnsOk()
        {
            // Arrange
            var request = new WeeklyStatusGetRequest { MemberId = 2, WeekStartDate = new DateTime(2023, 10, 17) };
            var mockWeeklyStatus = new Domain.DTOs.WeeklyStatusDTO();
            _mockService.Setup(service => service.GetWeeklyStatusByMemberByStartDateAsync((int)request.MemberId, request.WeekStartDate))
                        .ReturnsAsync(mockWeeklyStatus);

            // Act
            var result = await _controller.GetWeeklyStatusByMemberByStartDate(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(mockWeeklyStatus, okResult.Value);
        }

        [Fact]
        public async Task GetAllWeeklyStatusesByStartDate_WithData_ReturnsOk()
        {
            // Arrange
            var request = new WeeklyStatusGetRequest { WeekStartDate = new DateTime(2023, 10, 17) };
            var mockWeeklyStatusList = new List<WeeklyStatusWithMemberNameDTO> { new WeeklyStatusWithMemberNameDTO() };

            _mockService.Setup(service => service.GetAllWeeklyStatusesByStartDateAsync(It.IsAny<DateTime>()))
                        .ReturnsAsync(mockWeeklyStatusList); // Use ReturnsAsync with the correct list

            // Act
            var result = await _controller.GetAllWeeklyStatusesByStartDate(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<WeeklyStatusWithMemberNameDTO>>(okResult.Value);
            Assert.Equal(mockWeeklyStatusList, returnValue);
        }


        [Fact]
        public async Task SaveWeeklyStatus_WithValidData_ReturnsOk()
        {
            // Arrange
            var request = new WeeklyStatusPostRequest
            {
                MemberId = 1,
                WeekStartDate = new DateTime(2023, 10, 17),
                DoneThisWeek = new List<WebApi.DTOs.DoneThisWeekTaskDTO> { new WebApi.DTOs.DoneThisWeekTaskDTO { TaskDescription = "Task 1" } },
                PlanForNextWeek = new List<string> { "Plan 1" },
                Blockers = "None",
                UpcomingPTO = new List<DateTime> { new DateTime(2023, 10, 24) }
            };

            // Ensure that the DTO used here is the one from the Domain, as it seems to be the one expected by the service
            var mockAddedStatus = new TeamWeeklyStatus.Domain.DTOs.WeeklyStatusDTO
            {
                Id = 3,
                DoneThisWeek = request.DoneThisWeek.Select(dtw => new TeamWeeklyStatus.Domain.DTOs.DoneThisWeekTaskDTO { TaskDescription = dtw.TaskDescription }).ToList(),
                PlanForNextWeek = request.PlanForNextWeek,
                Blockers = request.Blockers,
                UpcomingPTO = request.UpcomingPTO,
                MemberId = request.MemberId
            };
            _mockService.Setup(service => service.AddWeeklyStatusAsync(It.IsAny<TeamWeeklyStatus.Domain.DTOs.WeeklyStatusDTO>()))
                        .ReturnsAsync(mockAddedStatus);

            // Act
            var result = await _controller.SaveWeeklyStatus(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(mockAddedStatus, okResult.Value);
        }



        [Fact]
        public async Task GetWeeklyStatusByMemberByStartDate_WithNonExistingData_ReturnsNotFound()
        {
            // Arrange
            var request = new WeeklyStatusGetRequest { MemberId = 2, WeekStartDate = new DateTime(2023, 10, 17) };
            _mockService.Setup(service => service.GetWeeklyStatusByMemberByStartDateAsync((int)request.MemberId, request.WeekStartDate))
                        .ReturnsAsync((Domain.DTOs.WeeklyStatusDTO)null);

            // Act
            var result = await _controller.GetWeeklyStatusByMemberByStartDate(request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
