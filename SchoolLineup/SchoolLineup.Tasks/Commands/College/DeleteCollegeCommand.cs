namespace SchoolLineup.Tasks.Commands.College
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class DeleteCollegeCommand : UnitOfWorkBaseCommand
    {
        public int EntityId { get; set; }
        private readonly ICollegeTasks tasks;

        public DeleteCollegeCommand(int id, ICollegeTasks tasks)
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