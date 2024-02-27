using Microsoft.EntityFrameworkCore;

namespace Zavrsni.TeamOps.Entity.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Organization> Organizations { get; } = new List<Organization>();
    }
}
