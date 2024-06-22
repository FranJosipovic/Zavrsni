using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.EF.Models;

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
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<Organization> Organizations { get; } = new List<Organization>();
        public List<Project> Projects { get; } = new List<Project>();
        public ICollection<ProjectWiki> ProjectWikis { get; } = new List<ProjectWiki>();  
        public ICollection<WorkItem> WorkItemsAssignedToMe { get; } = new List<WorkItem>();
    }
}
