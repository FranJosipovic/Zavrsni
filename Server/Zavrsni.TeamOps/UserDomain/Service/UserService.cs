using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.JWT;
using Zavrsni.TeamOps.UserDomain.Models;
using Zavrsni.TeamOps.UserDomain.Repository;
using Zavrsni.TeamOps.UserDomain.Validators;

namespace Zavrsni.TeamOps.UserDomain.Service
{
    public class UserService : IUserService
    {
        private readonly IUserValidator _userValidator;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserValidator userValidator, IUserRepository userRepository, IConfiguration configuration)
        {
            _userValidator = userValidator;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ServiceActionResult> SignUp(UserSignUpModel userSignUpModel)
        {
            var result = new ServiceActionResult();

            var validationResult = await _userValidator.ValidateUserSignUp(userSignUpModel.Username, userSignUpModel.Email, userSignUpModel.Password);

            if (!validationResult.IsValid)
            {
                result.SetBadRequest(validationResult.Messages[0]);
                return result;
            }

            var hashedPassword = HashPassword(userSignUpModel.Password);

            userSignUpModel.Password = hashedPassword;

            var dbResult = await _userRepository.PostAsync(userSignUpModel);

            result.Combine(dbResult);
            return result;
        }

        public async Task<ServiceActionResult> SignIn(UserSignInModel signInModel)
        {
            var result = new ServiceActionResult();

            var getUserResult = await _userRepository.GetAsync(signInModel.usernameOrEmail);
            result.Combine(getUserResult);
            if(getUserResult.IsSuccess)
            {
                try
                {
                    var user = (User)getUserResult.Data!;
                    var storedPasswordDetails = user.Password.Split(':');
                    var salt = Convert.FromBase64String(storedPasswordDetails[0]);
                    var storedHashedPassword = storedPasswordDetails[1];
                    var hashedPassword = HashPassword(signInModel.password, salt);

                    if (hashedPassword != storedHashedPassword)
                    {
                        result.SetAuthenticationFailed("Passwords do not match");
                    }
                    else
                    {
                        var jwt = JwtHelper.IssueNewToken(user,_configuration);
                        result.UpdateData(jwt);
                    }                
                }
                catch (Exception)
                {
                    result.SetInternalError();
                }
            }

            return result;
        }

        private string HashPassword(string password, byte[]? salt = null)
        {
            bool isNewSaltGenerated = false;
            if (salt == null)
            {
                salt = RandomNumberGenerator.GetBytes(128 / 8);
                isNewSaltGenerated = true;
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // If a new salt was generated, prepend it to the hashed password before returning.
            return isNewSaltGenerated ? $"{Convert.ToBase64String(salt)}:{hashed}" : hashed;
        }
    }
}
