using Moq;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.DTOs;


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
        public void GetMemberById_ReturnsCorrectMember()
        {
            // Arrange
            var expectedMember = new MemberDTO { Id = 1, Name = "John Doe" };
            _memberServiceMock.Setup(service => service.GetMemberById(1)).Returns(expectedMember);

            // Act
            var result = _memberServiceMock.Object.GetMemberById(1);

            // Assert
            Assert.Equal(expectedMember, result);
        }
    }
}