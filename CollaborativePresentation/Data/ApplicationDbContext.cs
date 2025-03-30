using CollaborativePresentation.Models;
using Microsoft.EntityFrameworkCore;

namespace CollaborativePresentation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<TextElement> TextElements { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Presentation>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<UserConnection>()
                .HasIndex(uc => new { uc.PresentationId, uc.UserName })
                .IsUnique();
        }
    }
}