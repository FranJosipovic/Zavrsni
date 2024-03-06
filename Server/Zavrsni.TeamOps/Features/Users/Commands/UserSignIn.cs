using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Features.Users.Models;
using Zavrsni.TeamOps.Features.Users.Utils;
using Zavrsni.TeamOps.JWT;

namespace Zavrsni.TeamOps.Features.Users.Commands
{
    public static class UserSignIn
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public string UsernameOrEmail { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        internal sealed class Handler : IRequestHandler<Command, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;
            private readonly IConfiguration _configuration;
            private readonly IMapper _mapper;
            public Handler(TeamOpsDbContext db, IConfiguration configuration, IMapper mapper)
            {
                _db = db;
                _configuration = configuration;
                _mapper = mapper;
            }

            public async Task<ServiceActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new ServiceActionResult();

                    //var user = await _userRepository.GetAsync(signInModel.usernameOrEmail);
                    var user = await _db.Users.Where(u => u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                    if (user is null)
                    {
                        result.SetNotFound("Couldn't find user with password or email");
                        return result;
                    }
                    var storedPasswordDetails = user.Password.Split(':');
                    var salt = Convert.FromBase64String(storedPasswordDetails[0]);
                    var storedHashedPassword = storedPasswordDetails[1];
                    var hashedPassword = PasswordHelper.HashPassword(request.Password, salt);

                    if (hashedPassword != storedHashedPassword)
                    {
                        result.SetAuthenticationFailed("Passwords do not match");
                        return result;
                    }
                    else
                    {
                        var jwt = JwtHelper.IssueNewToken(user, _configuration);
                        result.SetOk(new
                        {
                            Token = jwt,
                            User = _mapper.Map<UserNoSensitiveInfoDTO>(user)
                        }, "Successfully signed in");
                        return result;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
