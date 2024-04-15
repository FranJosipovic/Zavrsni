namespace Zavrsni.TeamOps.Common.Connectors.ResponseModels
{
    public class Organization
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string hooks_url { get; set; }
        public string issues_url { get; set; }
        public string members_url { get; set; }
        public string public_members_url { get; set; }
        public string avatar_url { get; set; }
        public object description { get; set; }
    }

    public class GetUserMembershipResponse
    {
        public string url { get; set; }
        public string state { get; set; }
        public string role { get; set; }
        public string organization_url { get; set; }
        public User user { get; set; }
        public Organization organization { get; set; }
    }

    public class User
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }
}
