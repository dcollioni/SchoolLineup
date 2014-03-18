namespace SchoolLineup.Tasks.Commands.School
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;

    public class CreateSchoolCommand : UnitOfWorkBaseCommand
    {
        public School Entity { get; set; }
        private readonly ISchoolTasks tasks;

        public CreateSchoolCommand(School entity, ISchoolTasks tasks)
        {
            this.Entity = entity;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}