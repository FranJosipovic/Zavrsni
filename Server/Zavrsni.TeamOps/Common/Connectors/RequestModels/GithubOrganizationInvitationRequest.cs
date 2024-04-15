namespace Zavrsni.TeamOps.Common.Connectors.RequestModels
{
    public class GithubOrganizationInvitationRequest
    {
        public int invitee_id { get; set; }
        public string role { get; set; }
    }
}
