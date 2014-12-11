namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;

    public interface IPartialGradeTasks
    {
        bool IsNameUnique(PartialGrade entity);
        bool IsOrderUnique(PartialGrade entity);
        bool HasChildren(int id);
    }
}