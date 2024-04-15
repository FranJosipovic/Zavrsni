using Zavrsni.TeamOps.Common.Connectors.RequestModels;

namespace Zavrsni.TeamOps.Common.Connectors
{
    public interface IGithubConnector
    {
        Task<HttpResponseMessage> GetUserMembership(string githubUser);
        Task<HttpResponseMessage> SearchUser(string q);
        Task<HttpResponseMessage> SendOrganizationInvitation(GithubOrganizationInvitationRequest request);
    }
}
