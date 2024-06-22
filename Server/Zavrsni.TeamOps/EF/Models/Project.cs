using Zavrsni.TeamOps.EF.Models;

namespace Zavrsni.TeamOps.Entity.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public ICollection<ProjectWiki> ProjectWikis { get; set; } = new List<ProjectWiki>();
        public ICollection<Iteration>  Iterrations { get; set; } = new List<Iteration>();
    }
}
