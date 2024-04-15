using MediatR;
using Zavrsni.TeamOps.Common.Connectors;
using Zavrsni.TeamOps.Common.Connectors.ResponseModels;

namespace Zavrsni.TeamOps.Features.Users.Queries
{
    public class GetUserMembershipStatus
    {
        public class Querry : IRequest<ServiceActionResult>
        {
            public string GithubUser { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Querry, ServiceActionResult>
        {
            private readonly IGithubConnector _githubConnector;
            public Handler(IGithubConnector githubConnector)
            {
                _githubConnector = githubConnector;
            }

            public async Task<ServiceActionResult> Handle(Querry request, CancellationToken cancellationToken)
            {
                var serviceActionResult = new ServiceActionResult();

                try
                {
                    var response = await _githubConnector.GetUserMembership(request.GithubUser);
                    response = response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<GetUserMembershipResponse>();
                    if (result is null) { serviceActionResult.SetInternalError(); }
                    else
                    {
                        serviceActionResult.SetOk(new { Status = result.state }, "action successfull");
                    }
                }
                catch (HttpRequestException e)
                {
                    if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        serviceActionResult.SetNotFound(e.Message);
                    }
                    else
                    {
                        serviceActionResult.SetInternalError();
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                return serviceActionResult;
            }
        }
    }
}
