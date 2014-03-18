namespace SchoolLineup.Tasks.Commands
{
    using SharpArch.Domain.Commands;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UnitOfWorkBaseCommand : CommandBase
    {
        public bool Success { get; set; }
        protected ICollection<ValidationResult> validationResults { get; set; }

        public UnitOfWorkBaseCommand()
        {
            validationResults = new List<ValidationResult>();
        }

        public override ICollection<ValidationResult> ValidationResults()
        {
            return validationResults;
        }

        public void SetValidationResults(ICollection<ValidationResult> validationResults)
        {
            this.validationResults = validationResults;
        }
    }
}