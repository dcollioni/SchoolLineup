namespace SchoolLineup.Tasks.Commands.ExamResult
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class SaveExamResultsCommand : UnitOfWorkBaseCommand
    {
        public IEnumerable<ExamResult> Entities { get; set; }
        private readonly IExamTasks tasks;

        public SaveExamResultsCommand(IEnumerable<ExamResult> entities, IExamTasks tasks)
        {
            this.Entities = entities;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            var examId = Entities.First().ExamId;
            var exam = tasks.Get(examId);

            foreach (var entity in Entities)
            {
                if (entity.ExamId <= 0)
                {
                    validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "ExamId" }));
                }

                if (entity.StudentId <= 0)
                {
                    validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "StudentId" }));
                }

                if (entity.Value > exam.Value)
                {
                    validationResults.Add(new ValidationResult(ResourceHelper.MustBeLessThan(exam.Value.ToString()), new[] { "Value" }));
                }
            }

            return (validationResults.Count == 0);
        }
    }
}