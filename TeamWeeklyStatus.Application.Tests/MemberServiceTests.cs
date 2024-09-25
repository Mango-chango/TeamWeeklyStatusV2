using Moq;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Tests
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberService> _memberServiceMock;
        public MemberServiceTests()
        {
            _memberServiceMock = new Mock<IMemberService>();
        }

        [Fact]
        public async Task GetMemberById_ReturnsCorrectMember() // Change method to async Task
        {
            // Arrange
            var expectedMember = new Member { Id = 1, Name = "John Doe" };
            _memberServiceMock.Setup(service => service.GetMemberByIdAsync(1)).ReturnsAsync(expectedMember); // Use ReturnsAsync

            // Act
            var result = await _memberServiceMock.Object.GetMemberByIdAsync(1); // Await the async method

            // Assert
            Assert.Equal(expectedMember, result);
        }
    }
}