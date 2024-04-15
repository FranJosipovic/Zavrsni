namespace Zavrsni.TeamOps.Common.Options
{
    public class GithubOptions
    {
        public static readonly string GitHubSettings = "GitHubSettings";
        public string AccessToken { get; set; } = string.Empty;
        public string XGitHubApiVersion { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }

}
