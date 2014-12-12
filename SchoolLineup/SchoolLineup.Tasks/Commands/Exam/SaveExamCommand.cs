namespace SchoolLineup.Tasks.Commands.Exam
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SaveExamCommand : UnitOfWorkBaseCommand
    {
        public Exam Entity { get; set; }
        private readonly IExamTasks tasks;

        public SaveExamCommand(Exam entity, IExamTasks tasks)
        {
            this.Entity = entity;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            if (Entity.PartialGradeId <= 0)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "PartialGradeId" }));
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

            if (Entity.Date == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Date" }));
            }

            if (Entity.Value <= 0)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Value" }));
            }

            return (validationResults.Count == 0);
        }
    }
}