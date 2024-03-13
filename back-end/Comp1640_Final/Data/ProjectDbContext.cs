using Comp1640_Final.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Data
{
    public class ProjectDbContext : IdentityDbContext<ApplicationUser>
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the foreign key constraint for StudentId in the Articles entity
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Student)
                .WithMany()  // Assuming ApplicationUser has a collection navigation property pointing back to Articles
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // Specify the behavior for delete operation

            // Configure the foreign key constraint for MarketingCoordinatorId in the Articles entity
            modelBuilder.Entity<Article>()
                .HasOne(a => a.MarketingCoordinator)
                .WithMany()  // Assuming ApplicationUser has a collection navigation property pointing back to Articles
                .HasForeignKey(a => a.MarketingCoordinatorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); // Specify the behavior for delete operation

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
        }
    }
}
