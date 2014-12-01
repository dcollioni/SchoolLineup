namespace SchoolLineup.Tasks.Commands.Course
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SaveCourseCommand : UnitOfWorkBaseCommand
    {
        public Course Entity { get; set; }
        private readonly ICourseTasks tasks;

        public SaveCourseCommand(Course entity, ICourseTasks tasks)
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

            if (Entity.CollegeId <= 0)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "CollegeId" }));
            }

            if (Entity.StartDate == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "StartDate" }));
            }

            if (Entity.FinishDate == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "FinishDate" }));
            }
            else if (Entity.FinishDate < Entity.StartDate)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.MustBeGreaterThan("Data inicial"), new[] { "FinishDate" }));
            }

            if (Entity.TeacherId <= 0)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "TeacherId" }));
            }

            return (validationResults.Count == 0);
        }
    }
}