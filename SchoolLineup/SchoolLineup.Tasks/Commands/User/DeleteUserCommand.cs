namespace SchoolLineup.Tasks.Commands.User
{
    using SchoolLineup.Domain.Contracts.Tasks;

    public class DeleteUserCommand : UnitOfWorkBaseCommand
    {
        public int EntityId { get; set; }
        private readonly IUserTasks tasks;

        public DeleteUserCommand(int id, IUserTasks tasks)
        {
            this.EntityId = id;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            //var hasChildren = tasks.HasChildren(EntityId);

            //if (hasChildren)
            //{
            //    validationResults.Add(new ValidationResult(ResourceHelper.HasChildren()));
            //}

            //return !hasChildren;

            return true;
        }
    }
}