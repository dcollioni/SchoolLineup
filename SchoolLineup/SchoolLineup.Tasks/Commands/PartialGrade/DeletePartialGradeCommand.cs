namespace SchoolLineup.Tasks.Commands.PartialGrade
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class DeletePartialGradeCommand : UnitOfWorkBaseCommand
    {
        public int EntityId { get; set; }
        private readonly IPartialGradeTasks tasks;

        public DeletePartialGradeCommand(int id, IPartialGradeTasks tasks)
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