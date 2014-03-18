namespace SchoolLineup.Tasks.Commands.School
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class SaveSchoolCommand : UnitOfWorkBaseCommand
    {
        public School Entity { get; set; }
        private readonly ISchoolTasks tasks;

        public SaveSchoolCommand(School entity, ISchoolTasks tasks)
        {
            this.Entity = entity;
            this.tasks = tasks;
        }

        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(Entity.Name))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Name" }));
            }
            else if (Entity.Name.Length > 50)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthField(50), new[] { "Name" }));
            }
            //else if (!stateTasks.IsUniqueName(Entity.Name, Entity.Id))
            //{
            //    validationResults.Add(new ValidationResult(ResourceHelper.AlreadyExists("Nome"), new[] { "Name" }));
            //}

            //if (string.IsNullOrEmpty(Entity.Code))
            //{
            //    validationResults.Add(new ValidationResult(ResourceHelper.RequiredFieldF("sigla"), new[] { "Code" }));
            //}
            //else if (Entity.Code.Length > 2)
            //{
            //    validationResults.Add(new ValidationResult(ResourceHelper.MaxLengthFieldF("sigla", 2), new[] { "Code" }));
            //}
            //else if (!stateTasks.IsUniqueCode(Entity.Code, Entity.Id))
            //{
            //    validationResults.Add(new ValidationResult(ResourceHelper.AlreadyExistsF("Sigla"), new[] { "Code" }));
            //}

            return (validationResults.Count == 0);
        }
    }
}