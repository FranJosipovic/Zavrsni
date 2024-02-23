namespace Zavrsni.TeamOps.Entity.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<User> Users { get; }
    }
}
