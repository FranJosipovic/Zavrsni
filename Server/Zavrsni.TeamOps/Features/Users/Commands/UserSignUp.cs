using AutoMapper;
using MediatR;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Users.Models.DTOs;
using Zavrsni.TeamOps.Features.Users.Utils;
using Zavrsni.TeamOps.Features.Users.Validators;

namespace Zavrsni.TeamOps.Features.Users.Commands
{
    public static class UserSignUp
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public int InviteeId {  get; set; }
        }

        //public class Validator : AbstractValidator<Command>
        //{
        //    public Validator()
        //    {
        //        RuleFor(c => c.Title).NotEmpty();
        //        RuleFor(c => c.Content).NotEmpty();
        //    }
        //}

        internal sealed class Handler : IRequestHandler<Command, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _dbContext;
            //private readonly IValidator<Command> _validator;
            private readonly IUserValidator _userValidator;
            private readonly IMapper _mapper;
            public Handler(TeamOpsDbContext dbContext, /*IValidator<Command> validator,*/ IMapper mapper)
            {
                _dbContext = dbContext;
                //_validator = validator;
                _userValidator = new UserValidator(_dbContext);
                _mapper = mapper;
            }

            public async Task<ServiceActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var serviceActionResult = new ServiceActionResult();
                //var validationResult = _validator.Validate(request);
                //if (!validationResult.IsValid)
                //{
                //    serviceActionResult.SetBadRequest("Bad request");
                //    return serviceActionResult;
                //}
                var validationResult = await _userValidator.ValidateUserSignUp(request.Username, request.Email, request.Password);
                if (!validationResult.IsValid)
                {
                    serviceActionResult.SetBadRequest(validationResult.Messages[0]);
                    return serviceActionResult;
                }

                var hashedPassword = PasswordHelper.HashPassword(request.Password);

                request.Password = hashedPassword;

                User user = _mapper.Map<User>(request);
                try
                {
                    await _dbContext.Users.AddAsync(user, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception)
                {
                    throw;
                }

                UserNoSensitiveInfoDTO createdUser = _mapper.Map<UserNoSensitiveInfoDTO>(user);

                serviceActionResult.SetResultCreated(new
                {
                    User = createdUser
                });
                return serviceActionResult;
            }
        }
    }
}
