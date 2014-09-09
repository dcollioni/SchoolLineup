namespace SchoolLineup.Web.Mvc.Controllers.Api
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.Queries.School;
    using System.Collections.Generic;
    using System.Web.Http;

    public class SchoolsController : ApiController
    {
        private readonly ISchoolListQuery schoolListQuery;

        public SchoolsController(ISchoolListQuery schoolListQuery)
        {
            this.schoolListQuery = schoolListQuery;
        }

        // GET api/<controller>
        public IEnumerable<School> Get()
        {
            return schoolListQuery.GetAllEntities();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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