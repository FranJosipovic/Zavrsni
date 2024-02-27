using System.Globalization;
using System.Text.RegularExpressions;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.UserDomain.Repository;
using Zavrsni.TeamOps.Validation;

namespace Zavrsni.TeamOps.UserDomain.Validators
{
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository _userRepository;

        public UserValidator(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<ValidationResult> ValidateUserSignUp(string username, string email, string password)
        {
            //validate username is uniqu
            //validate email is unique and is email
            //password and validate(min 8 chars, at least 1 num, at least one special char)
            var validationHandler = new ValidationHandler();

            var isUsernameUnique = await _userRepository.IsUnique(
                (User u) => u.Username,
                username);

            var isEmailUnique = await _userRepository.IsUnique(
                (User u) => u.Email,
                email);

            validationHandler.Validate(isUsernameUnique, "Username is not unique")
                .Validate(isEmailUnique, "Email is not unique")
                .Validate(IsValidEmail(email), "Email is invalid")
                .Validate(IsValidPassword(password), "Password does not meet requirements");

            return validationHandler.Result;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private bool IsValidPassword(string password)
        {
            // Check if the password is null or whitespace
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Regex explanation:
            // ^                         Start anchor
            // (?=.*[0-9])               At least one digit
            // (?=.*[!@#$%^&*])          At least one special character
            // (?=.*[a-zA-Z])            At least one letter (either lowercase or uppercase)
            // .{8,}                     At least 8 characters
            // $                         End anchor
            var regex = new Regex(@"^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-zA-Z]).{8,}$");

            return regex.IsMatch(password);
        }
    }
}
