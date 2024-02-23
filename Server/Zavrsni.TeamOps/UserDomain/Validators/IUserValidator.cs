namespace Zavrsni.TeamOps.UserDomain.Validators
{
    public interface IUserValidator
    {
        Task<ValidationResult> ValidateUserSignUp(string username, string email, string password);
    }
}
