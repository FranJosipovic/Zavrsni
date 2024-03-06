using Zavrsni.TeamOps.Validation;

namespace Zavrsni.TeamOps.Features.Users.Validators
{
    public interface IUserValidator
    {
        Task<ValidationResult> ValidateUserSignUp(string username, string email, string password);
    }
}
