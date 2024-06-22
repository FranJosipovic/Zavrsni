using System.Reflection.Metadata.Ecma335;

namespace Zavrsni.TeamOps.Features.Projects.Models
{
    public class ProjectDetailsDTO
    {
        public string Title { get; set; }
        public int Users { get; set; }
        public int Iterations { get; set; }
        public int UserStories { get; set; }
        public int Task_bugs { get; set; }
    }
}
