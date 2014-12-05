namespace SchoolLineup.Tasks.Commands.Student
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using SchoolLineup.Util;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ImportStudentsCommand : UnitOfWorkBaseCommand
    {
        public IEnumerable<Student> Entities { get; set; }
        private readonly IStudentTasks tasks;

        public ImportStudentsCommand(IEnumerable<Student> entities, IStudentTasks tasks)
        {
            this.Entities = entities;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            foreach (var entity in Entities)
            {
                if (string.IsNullOrEmpty(entity.Name))
                {
                    validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Name" }));
                }
                else if (entity.Name.Length > 50)
                {
                    validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthField(50), new[] { "Name" }));
                }

                if (string.IsNullOrEmpty(entity.Email))
                {
                    validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Email" }));
                }
                else if (entity.Email.Length > 100)
                {
                    validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthField(100), new[] { "Email" }));
                }
                else if (!Validation.IsEmailValid(entity.Email))
                {
                    validationResults.Add(new ValidationResult(ResourceHelper.InvalidEmail(), new[] { "Email" }));
                }
            }

            var emails = Entities.Select(e => e.Email);

            if (!tasks.AreEmailsUnique(emails))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.UniqueField(), new[] { "Email" }));
            }

            return (validationResults.Count == 0);
        }
    }
}