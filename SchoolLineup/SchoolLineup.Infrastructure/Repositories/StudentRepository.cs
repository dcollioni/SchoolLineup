namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;

    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IDocumentSession session)
            : base(session)
        {
        }
    }
}