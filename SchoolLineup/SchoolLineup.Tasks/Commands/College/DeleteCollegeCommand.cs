namespace SchoolLineup.Tasks.Commands.College
{
    using SchoolLineup.Domain.Contracts.Tasks;

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
            return true;
        }
    }
}