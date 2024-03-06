using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Features.Users.Models;

namespace Zavrsni.TeamOps.Features.Users.Queries
{
    public static class GetUsersByOrganization
    {
        public class Querry : IRequest<ServiceActionResult>
        {
            public Guid OrganizationId { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Querry, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;
            private readonly IMapper _mapper;
            public Handler(TeamOpsDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<ServiceActionResult> Handle(Querry request, CancellationToken cancellationToken)
            {
                try
                {
                    var serviceActionResult = new ServiceActionResult();
                    var organization = await _db.Organizations.FindAsync(new object?[] { request.OrganizationId }, cancellationToken: cancellationToken);
                    if (organization is null)
                    {
                        serviceActionResult.SetBadRequest("Organization not found");
                        return serviceActionResult;
                    }

                    var users = await _db.Users.Where(u => u.Organizations.Contains(organization))
                        .Select(u => _mapper.Map<UserNoSensitiveInfoDTO>(u))
                        .ToListAsync(cancellationToken);

                    serviceActionResult.SetOk(new CollectionResponseData<UserNoSensitiveInfoDTO>
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
