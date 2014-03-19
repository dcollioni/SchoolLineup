namespace SchoolLineup.Tasks.Commands.School
{
    using SchoolLineup.Domain.Contracts.Tasks;

    public class DeleteSchoolCommand : UnitOfWorkBaseCommand
    {
        public int EntityId { get; set; }
        private readonly ISchoolTasks tasks;

        public DeleteSchoolCommand(int id, ISchoolTasks tasks)
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