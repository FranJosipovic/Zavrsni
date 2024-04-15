namespace Zavrsni.TeamOps.Features.Users.Models
{
    public class UserNoSensitiveInfoDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string GitHubUser { get; set; } = string.Empty;
    }
}
