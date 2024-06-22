using MediatR;
using Zavrsni.TeamOps.Common.Connectors.ResponseModels;
using Zavrsni.TeamOps.Common.Connectors;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.Entity;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Features.Users.Models.DTOs;

namespace Zavrsni.TeamOps.Features.Users.Queries
{
    public class SearchUsers
    {
        public class Querry : IRequest<ServiceActionResult>
        {
            public string q { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Querry, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;

            public Handler(TeamOpsDbContext db)
            {
                _db = db;
            }

            public async Task<ServiceActionResult> Handle(Querry request, CancellationToken cancellationToken)
            {

                try
                {
                    var serviceActionResult = new ServiceActionResult();
                    var users = await _db.Users
                        .Where(u => u.Username.StartsWith(request.q) || u.Name.StartsWith(request.q) || u.Surname.StartsWith(request.q) || u.Email.StartsWith(request.q))
                        .Select(u => new SearchedUserDTO
                        {
                            Surname = u.Username,
                            Email = u.Email,
                            Id = u.Id,
                            Name = u.Name,
                            Username = u.Username,
                        }).ToListAsync();

                    serviceActionResult.SetOk(new CollectionResponseData<SearchedUserDTO>
                    {
                        Items = users,
                        Count = users.Count
                    }, "Action successfull");
                    return serviceActionResult;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
