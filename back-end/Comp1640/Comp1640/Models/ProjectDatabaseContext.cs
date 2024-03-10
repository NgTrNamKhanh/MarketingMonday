using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models
{
    public class ProjectDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public ProjectDatabaseContext(DbContextOptions<ProjectDatabaseContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
    }
}
