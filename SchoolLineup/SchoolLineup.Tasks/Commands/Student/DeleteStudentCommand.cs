namespace SchoolLineup.Tasks.Commands.Student
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class DeleteStudentCommand : UnitOfWorkBaseCommand
    {
        public int EntityId { get; set; }
        private readonly IStudentTasks tasks;

        public DeleteStudentCommand(int id, IStudentTasks tasks)
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