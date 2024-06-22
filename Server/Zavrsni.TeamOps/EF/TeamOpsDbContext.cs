using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.Entity
{
    public class TeamOpsDbContext : DbContext
    {
        public TeamOpsDbContext(DbContextOptions<TeamOpsDbContext> options) : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectWiki> ProjectWikis { get; set; }
        public DbSet<Iteration> Iterations { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>()
                        .HasOne(o => o.Owner)
                        .WithMany()
                        .HasForeignKey(o => o.OwnerId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                        .HasOne<Organization>()
                        .WithMany(p => p.Projects)
                        .HasForeignKey(fk => fk.OrganizationId)
                        .IsRequired();


            modelBuilder.Entity<ProjectWiki>()
                        .HasOne(e => e.CreatedBy)
                        .WithMany(e => e.ProjectWikis)
                        .HasForeignKey(e => e.CreatedById)
                        .IsRequired();

            modelBuilder.Entity<ProjectWiki>()
                        .HasOne(e => e.Parent)
                        .WithMany(e => e.Children)
                        .HasForeignKey(e => e.ParentId)
                        .IsRequired(false);

            modelBuilder.Entity<ProjectWiki>()
                        .HasOne(e => e.Project)
                        .WithMany(p => p.ProjectWikis)
                        .HasForeignKey(e => e.ProjectId)
                        .IsRequired();

            modelBuilder.Entity<WorkItem>()
                        .HasOne(e => e.Iteration)
                        .WithMany(p => p.WorkItems)
                        .HasForeignKey(e => e.IterationId)
                        .IsRequired();

            modelBuilder.Entity<WorkItem>()
                        .HasOne(e => e.Parent)
                        .WithMany(e => e.Children)
                        .HasForeignKey(e => e.ParentId)
                        .IsRequired(false);

            modelBuilder.Entity<WorkItem>()
                        .HasOne(e => e.AssignedTo)
                        .WithMany(e => e.WorkItemsAssignedToMe)
                        .HasForeignKey(e => e.AssignedToId)
                        .IsRequired(false);
        }
    }
}
