namespace SchoolLineup.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using SchoolLineup.Tasks.CommandHandlers.School;
    using SchoolLineup.Tasks.Commands.School;
    using System.Linq;

    [TestClass]
    public class SaveSchoolHandlerTest
    {
        [TestMethod]
        public void Save_School_Name_Is_Null_Returns_Error()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var schoolRepository = mockRepository.Create<ISchoolRepository>();
            schoolRepository.Setup(x => x.Evict(It.IsAny<School>()));

            var command = new SaveSchoolCommand(new School(), null);

            var expectedError = ResourceHelper.RequiredField();

            // Act
            var handler = new SaveSchoolHandler(schoolRepository.Object);
            handler.Handle(command);

            // Assert
            Assert.IsFalse(command.Success);
            Assert.IsTrue(command.ValidationResults().Any(r => r.ErrorMessage == expectedError));
        }

        [TestMethod]
        public void Save_School_Name_Length_Is_Greater_Than_50_Returns_Error()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var schoolRepository = mockRepository.Create<ISchoolRepository>();
            schoolRepository.Setup(x => x.Evict(It.IsAny<School>()));

            var command = new SaveSchoolCommand(new School() { Name = new string('a', 51) }, null);

            var expectedError = ResourceHelper.MaxLengthField(50);

            // Act
            var handler = new SaveSchoolHandler(schoolRepository.Object);
            handler.Handle(command);

            // Assert
            Assert.IsFalse(command.Success);
            Assert.IsTrue(command.ValidationResults().Any(r => r.ErrorMessage == expectedError));
        }
    }
}