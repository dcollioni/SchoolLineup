namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;

    public interface IStudentTasks
    {
        bool IsEmailUnique(Student entity);
        bool HasChildren(int id);
    }
}