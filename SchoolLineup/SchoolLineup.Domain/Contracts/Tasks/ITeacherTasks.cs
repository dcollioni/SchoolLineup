namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;

    public interface ITeacherTasks
    {
        bool IsEmailUnique(Teacher entity);
        bool HasChildren(int id);
    }
}