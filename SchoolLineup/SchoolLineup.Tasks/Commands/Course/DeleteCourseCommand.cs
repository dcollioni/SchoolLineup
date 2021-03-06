﻿namespace SchoolLineup.Tasks.Commands.Course
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class DeleteCourseCommand : UnitOfWorkBaseCommand
    {
        public int EntityId { get; set; }
        private readonly ICourseTasks tasks;

        public DeleteCourseCommand(int id, ICourseTasks tasks)
        {
            this.EntityId = id;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            var hasChildren = tasks.HasChildren(EntityId);

            if (hasChildren)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.HasChildren()));
            }

            return !hasChildren;
        }
    }
}