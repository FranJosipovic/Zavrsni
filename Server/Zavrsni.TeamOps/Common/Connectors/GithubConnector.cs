using System.Text.Json;
using System.Text;
using Zavrsni.TeamOps.Common.Connectors.RequestModels;

namespace Zavrsni.TeamOps.Common.Connectors
{
    public class GithubConnector : IGithubConnector
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GithubConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetUserMembership(string githubUser)
        {
            var client = _httpClientFactory.CreateClient("github");
            var message = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/orgs/TeamOpsOrg/memberships/{githubUser}");
            var response = await client.SendAsync(message);
            return response;
        }

        public async Task<HttpResponseMessage> SearchUser(string q)
        {
            var client = _httpClientFactory.CreateClient("github");
            var message = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/search/users?q={q}");
            var response = await client.SendAsync(message);
            return response;
        }

        public async Task<HttpResponseMessage> SendOrganizationInvitation(GithubOrganizationInvitationRequest request)
        {
            var client = _httpClientFactory.CreateClient("github");
            var message = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/orgs/TeamOpsOrg/invitations");
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            message.Content = content;
            var response = await client.SendAsync(message);
            return response;
        }
    }
}
