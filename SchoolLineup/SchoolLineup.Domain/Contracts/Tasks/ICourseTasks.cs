namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;

    public interface ICourseTasks
    {
        bool HasChildren(int id);
    }
}