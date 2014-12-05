namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;
    using System.Collections.Generic;

    public interface IStudentTasks
    {
        bool IsEmailUnique(Student entity);
        bool AreEmailsUnique(IEnumerable<string> emails);
        bool HasChildren(int id);
    }
}