namespace SchoolLineup.Tasks.Commands.School
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class SaveSchoolCommand : UnitOfWorkBaseCommand
    {
        public School Entity { get; set; }
        private readonly ISchoolTasks tasks;

        public SaveSchoolCommand(School entity, ISchoolTasks tasks)
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
            else if (Entity.Email.Length > 50)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthField(50), new[] { "Email" }));
            }

            if (!string.IsNullOrEmpty(Entity.ManagerName) && Entity.ManagerName.Length > 50)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthField(50), new[] { "ManagerName" }));
            }

            if (!string.IsNullOrEmpty(Entity.Phone) && Entity.Phone.Length > 12)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthField(12), new[] { "Phone" }));
            }

            return (validationResults.Count == 0);
        }
    }
}