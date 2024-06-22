using MediatR;
using Zavrsni.TeamOps.Entity;

namespace Zavrsni.TeamOps.Features.WorkItems.Commands
{
    public class DeleteWorkItem
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;
            public Handler(TeamOpsDbContext db)
            {
                _db = db;
            }

            public async Task<ServiceActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new ServiceActionResult();

                    var workItem = await _db.WorkItems.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
                    if (workItem == null)
                    {
                        result.SetBadRequest("Work item does not exists");
                        return result;
                    }
                    _db.WorkItems.Remove(workItem);
                    await _db.SaveChangesAsync(cancellationToken);
                    result.SetOk(null, "successfully deleted");
                    return result;

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
