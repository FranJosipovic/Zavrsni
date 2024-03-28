using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>();
            modelBuilder.Entity<User>();

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
        }
    }
}
