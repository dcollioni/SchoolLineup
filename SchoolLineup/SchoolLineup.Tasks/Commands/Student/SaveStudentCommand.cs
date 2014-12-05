namespace SchoolLineup.Tasks.Commands.Student
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using SchoolLineup.Util;
    using System.ComponentModel.DataAnnotations;

    public class SaveStudentCommand : UnitOfWorkBaseCommand
    {
        public Student Entity { get; set; }
        private readonly IStudentTasks tasks;

        public SaveStudentCommand(Student entity, IStudentTasks tasks)
        {
            this.Entity = entity;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(Entity.Name))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Name" }));
            }
            else if (Entity.Name.Length > 50)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthField(50), new[] { "Name" }));
            }

            if (string.IsNullOrEmpty(Entity.Email))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Email" }));
            }
            else if (Entity.Email.Length > 100)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthField(100), new[] { "Email" }));
            }
            else if (!Validation.IsEmailValid(Entity.Email))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.InvalidEmail(), new[] { "Email" }));
            }
            else if (!tasks.IsEmailUnique(Entity))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.UniqueField(), new[] { "Email" }));
            }

            return (validationResults.Count == 0);
        }
    }
}