namespace SchoolLineup.Web.Mvc.Controllers.Api
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.Queries.ExamResult;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Web.Http;

    public class ExamResultsController : ApiController
    {
        private readonly IExamResultListQuery examResultListQuery;

        public ExamResultsController(IExamResultListQuery examResultListQuery)
        {
            this.examResultListQuery = examResultListQuery;
        }

        // GET api/<controller>
        public IEnumerable<ExamResult> Get()
        {
            return null;
        }

        // GET api/<controller>/5
        public IEnumerable<ExamResultViewModel> Get(int studentId, int partialGradeId)
        {
            return examResultListQuery.GetAll(studentId, partialGradeId);
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