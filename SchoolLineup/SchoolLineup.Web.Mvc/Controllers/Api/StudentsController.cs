namespace SchoolLineup.Web.Mvc.Controllers.Api
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.Queries.School;
    using System.Collections.Generic;
    using System.Web.Http;

    public class StudentsController : ApiController
    {
        private readonly IStudentListQuery studentListQuery;

        public StudentsController(IStudentListQuery studentListQuery)
        {
            this.studentListQuery = studentListQuery;
        }

        // GET api/<controller>
        public IEnumerable<School> Get()
        {
            return null;
        }

        // GET api/<controller>/5
        public Student Get(string email, string password)
        {
            return studentListQuery.Get(email, password);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}