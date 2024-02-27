namespace Zavrsni.TeamOps.Entity.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new List<User>();
    }
}
