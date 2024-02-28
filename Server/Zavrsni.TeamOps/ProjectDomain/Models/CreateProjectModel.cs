using System.ComponentModel.DataAnnotations;

namespace Zavrsni.TeamOps.ProjectDomain.Models
{
    public class CreateProjectModel
    {
        [Required]
        public Guid OrganizationId { get; set; }
        [Required]
        public Guid OrganizationOwnerId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
