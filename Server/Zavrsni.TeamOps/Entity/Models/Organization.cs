namespace Zavrsni.TeamOps.Entity.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId {  get; set; }
        public User Owner { get; set; }
        public List<User> Users { get; } = new List<User>();
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
