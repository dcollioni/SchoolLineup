namespace SchoolLineup.Tasks.Commands.Account
{
    using SchoolLineup.Domain.Resources;
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordCommand : UnitOfWorkBaseCommand
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }

        public ChangePasswordCommand(int userId, string password, string passwordConfirmation)
        {
            this.UserId = userId;
            this.Password = password;
            this.PasswordConfirmation = passwordConfirmation;
        }

        public override bool IsValid()
        {
            if (UserId <= 0)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "UserId" }));
            }

            if (string.IsNullOrEmpty(Password))
            {
                validationResults.Add(new ValidationResult(ResourceHelper.RequiredField(), new[] { "Password" }));
            }
            else if (Password.Length < 6)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.MinLengthField(6), new[] { "Password" }));
            }
            else if (Password != PasswordConfirmation)
            {
                validationResults.Add(new ValidationResult(ResourceHelper.PasswordsMustBeEqual(), new[] { "Password" }));
            }

            return (validationResults.Count == 0);
        }
    }
}