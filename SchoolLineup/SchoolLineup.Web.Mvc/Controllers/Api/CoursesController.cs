namespace SchoolLineup.Web.Mvc.Controllers.Api
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Course;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Web.Http;

    public class CoursesController : ApiController
    {
        private readonly ICourseListQuery courseListQuery;

        public CoursesController(ICourseListQuery courseListQuery)
        {
            this.courseListQuery = courseListQuery;
        }

        // GET api/<controller>
        public IEnumerable<Course> Get()
        {
            return null;
        }

        // GET api/<controller>/5
        public IEnumerable<CourseViewModel> Get(int studentId)
        {
            return courseListQuery.GetAll(studentId);
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