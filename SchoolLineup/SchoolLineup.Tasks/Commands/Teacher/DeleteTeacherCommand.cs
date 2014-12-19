namespace SchoolLineup.Tasks.Commands.Teacher
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class DeleteTeacherCommand : UnitOfWorkBaseCommand
    {
        public int EntityId { get; set; }
        private readonly ITeacherTasks tasks;

        public DeleteTeacherCommand(int id, ITeacherTasks tasks)
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