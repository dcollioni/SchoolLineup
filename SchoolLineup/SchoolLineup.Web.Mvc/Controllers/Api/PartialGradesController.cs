namespace SchoolLineup.Web.Mvc.Controllers.Api
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.Queries.PartialGrade;
    using System.Collections.Generic;
    using System.Web.Http;

    public class PartialGradesController : ApiController
    {
        private readonly IPartialGradeListQuery partialGradeListQuery;

        public PartialGradesController(IPartialGradeListQuery partialGradeListQuery)
        {
            this.partialGradeListQuery = partialGradeListQuery;
        }

        // GET api/<controller>/5
        public IEnumerable<PartialGrade> Get(int courseId)
        {
            return partialGradeListQuery.GetAll(courseId);
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