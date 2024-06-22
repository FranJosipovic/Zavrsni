using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Features.Users.Models.DTOs;

namespace Zavrsni.TeamOps.Features.Users.Queries
{
    public static class GetUsersByProject
    {
        public class Querry : IRequest<ServiceActionResult>
        {
            public Guid ProjectId { get; set; }
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
                    var project = await _db.Projects.FindAsync(new object?[] { request.ProjectId }, cancellationToken: cancellationToken);
                    if (project is null)
                    {
                        serviceActionResult.SetBadRequest("Project does not exists");
                        return serviceActionResult;
                    }

                    var users = await _db.Users.Where(u => u.Projects.Contains(project))
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
