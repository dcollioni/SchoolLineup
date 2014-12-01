namespace SchoolLineup.Tasks.Commands.College
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class SaveCollegeCommand : UnitOfWorkBaseCommand
    {
        public College Entity { get; set; }
        private readonly ICollegeTasks tasks;

        public SaveCollegeCommand(College entity, ICollegeTasks tasks)
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
            else if (!tasks.IsNameUnique(Entity))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.UniqueField(), new[] { "Name" }));
            }

            return (validationResults.Count == 0);
        }
    }
}