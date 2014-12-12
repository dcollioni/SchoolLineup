namespace SchoolLineup.Tasks.Commands.Exam
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class DeleteExamCommand : UnitOfWorkBaseCommand
    {
        public int EntityId { get; set; }
        private readonly IExamTasks tasks;

        public DeleteExamCommand(int id, IExamTasks tasks)
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

            return (validationResults.Count == 0);
        }
    }
}