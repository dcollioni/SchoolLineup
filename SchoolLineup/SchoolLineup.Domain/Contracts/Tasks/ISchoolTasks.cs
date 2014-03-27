namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;

    public interface ISchoolTasks
    {
        bool IsNameUnique(School entity);
    }
}