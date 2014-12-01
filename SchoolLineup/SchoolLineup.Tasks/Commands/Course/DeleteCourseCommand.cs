namespace SchoolLineup.Tasks.Commands.Course
{
    using SchoolLineup.Domain.Contracts.Tasks;

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
            return true;
        }
    }
}