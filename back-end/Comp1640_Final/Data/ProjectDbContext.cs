using Comp1640_Final.Models;
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

        
    }
}
