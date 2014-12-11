namespace SchoolLineup.Tasks.Commands.PartialGrade
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class SavePartialGradeCommand : UnitOfWorkBaseCommand
    {
        public PartialGrade Entity { get; set; }
        private readonly IPartialGradeTasks tasks;

        public SavePartialGradeCommand(PartialGrade entity, IPartialGradeTasks tasks)
        {
            this.Entity = entity;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            if (Entity.CourseId <= 0)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "CourseId" }));
            }

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

            if (Entity.Order <= 0)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Order" }));
            }
            else if (!tasks.IsOrderUnique(Entity))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.UniqueField(), new[] { "Order" }));
            }

            return (validationResults.Count == 0);
        }
    }
}