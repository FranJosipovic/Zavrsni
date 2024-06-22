using System.Data.Entity.Migrations.Model;
using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.EF.Models
{
    public class ProjectWiki
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid CreatedById { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public User CreatedBy { get; set; }
        public ProjectWiki? Parent { get; set; }
        public ICollection<ProjectWiki> Children { get; } = new List<ProjectWiki>();
        public Project Project { get; set; }
    }
}
