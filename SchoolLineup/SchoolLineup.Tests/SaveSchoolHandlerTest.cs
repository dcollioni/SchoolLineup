namespace SchoolLineup.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using SchoolLineup.Tasks;
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

        [TestMethod]
        public void Save_School_Name_Is_Not_Unique_Returns_Error()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var schoolRepository = mockRepository.Create<ISchoolRepository>();
            schoolRepository.Setup(x => x.Evict(It.IsAny<School>()));
            schoolRepository.Setup(x => x.CountByName(It.IsAny<School>())).Returns(1);

            var schoolTasks = new SchoolTasks(schoolRepository.Object);

            var command = new SaveSchoolCommand(new School() { Name = "Escola 1" }, schoolTasks);

            var expectedError = ResourceHelper.UniqueField();

            // Act
            var handler = new SaveSchoolHandler(schoolRepository.Object);
            handler.Handle(command);

            // Assert
            Assert.IsFalse(command.Success);
            Assert.IsTrue(command.ValidationResults().Any(r => r.ErrorMessage == expectedError));
        }

        [TestMethod]
        public void Save_School_Email_Is_Null_Returns_Error()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var schoolRepository = mockRepository.Create<ISchoolRepository>();
            schoolRepository.Setup(x => x.Evict(It.IsAny<School>()));
            schoolRepository.Setup(x => x.CountByName(It.IsAny<School>())).Returns(0);

            var schoolTasks = new SchoolTasks(schoolRepository.Object);

            var entity = new School() { Name = "Escola 1" };
            var command = new SaveSchoolCommand(entity, schoolTasks);

            var expectedError = ResourceHelper.RequiredField();

            // Act
            var handler = new SaveSchoolHandler(schoolRepository.Object);
            handler.Handle(command);

            // Assert
            Assert.IsFalse(command.Success);
            Assert.IsTrue(command.ValidationResults().Any(r => r.ErrorMessage == expectedError));
        }

        [TestMethod]
        public void Save_School_Email_Length_Is_Greater_Than_50_Returns_Error()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var schoolRepository = mockRepository.Create<ISchoolRepository>();
            schoolRepository.Setup(x => x.Evict(It.IsAny<School>()));
            schoolRepository.Setup(x => x.CountByName(It.IsAny<School>())).Returns(0);

            var schoolTasks = new SchoolTasks(schoolRepository.Object);

            var entity = new School() { Name = "Escola 1", Email = new string('a', 51) };
            var command = new SaveSchoolCommand(entity, schoolTasks);

            var expectedError = ResourceHelper.MaxLengthField(50);

            // Act
            var handler = new SaveSchoolHandler(schoolRepository.Object);
            handler.Handle(command);

            // Assert
            Assert.IsFalse(command.Success);
            Assert.IsTrue(command.ValidationResults().Any(r => r.ErrorMessage == expectedError));
        }

        [TestMethod]
        public void Save_School_Email_Is_Not_Valid_Returns_Error()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var schoolRepository = mockRepository.Create<ISchoolRepository>();
            schoolRepository.Setup(x => x.Evict(It.IsAny<School>()));
            schoolRepository.Setup(x => x.CountByName(It.IsAny<School>())).Returns(0);

            var schoolTasks = new SchoolTasks(schoolRepository.Object);

            var entity = new School() { Name = "Escola 1", Email = "emailwithoutatsymbol" };
            var command = new SaveSchoolCommand(entity, schoolTasks);

            var expectedError = ResourceHelper.InvalidEmail();

            // Act
            var handler = new SaveSchoolHandler(schoolRepository.Object);
            handler.Handle(command);

            // Assert
            Assert.IsFalse(command.Success);
            Assert.IsTrue(command.ValidationResults().Any(r => r.ErrorMessage == expectedError));
        }

        [TestMethod]
        public void Save_School_ManagerName_Length_Is_Greater_Than_50_Returns_Error()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var schoolRepository = mockRepository.Create<ISchoolRepository>();
            schoolRepository.Setup(x => x.Evict(It.IsAny<School>()));
            schoolRepository.Setup(x => x.CountByName(It.IsAny<School>())).Returns(0);

            var schoolTasks = new SchoolTasks(schoolRepository.Object);

            var entity = new School() { Name = "Escola 1", Email = "escola@gmail.com", ManagerName = new string('a', 51) };
            var command = new SaveSchoolCommand(entity, schoolTasks);

            var expectedError = ResourceHelper.MaxLengthField(50);

            // Act
            var handler = new SaveSchoolHandler(schoolRepository.Object);
            handler.Handle(command);

            // Assert
            Assert.IsFalse(command.Success);
            Assert.IsTrue(command.ValidationResults().Any(r => r.ErrorMessage == expectedError));
        }

        [TestMethod]
        public void Save_School_Phone_Length_Is_Greater_Than_12_Returns_Error()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var schoolRepository = mockRepository.Create<ISchoolRepository>();
            schoolRepository.Setup(x => x.Evict(It.IsAny<School>()));
            schoolRepository.Setup(x => x.CountByName(It.IsAny<School>())).Returns(0);

            var schoolTasks = new SchoolTasks(schoolRepository.Object);

            var entity = new School() { Name = "Escola 1", Email = "escola@gmail.com", ManagerName = "João da Silva", Phone = new string('9', 13) };
            var command = new SaveSchoolCommand(entity, schoolTasks);

            var expectedError = ResourceHelper.MaxLengthField(12);

            // Act
            var handler = new SaveSchoolHandler(schoolRepository.Object);
            handler.Handle(command);

            // Assert
            Assert.IsFalse(command.Success);
            Assert.IsTrue(command.ValidationResults().Any(r => r.ErrorMessage == expectedError));
        }
    }
}