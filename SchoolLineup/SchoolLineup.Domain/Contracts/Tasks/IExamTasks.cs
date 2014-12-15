namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;

    public interface IExamTasks
    {
        bool IsNameUnique(Exam entity);
        bool HasChildren(int id);
        Exam Get(int id);
    }
}