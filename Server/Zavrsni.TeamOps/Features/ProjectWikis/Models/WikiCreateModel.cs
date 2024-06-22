namespace Zavrsni.TeamOps.Features.ProjectWikis.Models
{
    public class WikiCreateModel
    {
        public string Title{ get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? ParentId { get; set; }
    }
}
