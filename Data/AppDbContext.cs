using Microsoft.EntityFrameworkCore;

namespace Book.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Boook> Books {get; set;}
        public DbSet<User> Users {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Boook>(enity =>
            {
                enity.HasKey(b => b.BookId);
                enity.Property(b => b.BookName).IsRequired().HasMaxLength(200);
                enity.Property(b => b.AuthorName).IsRequired().HasMaxLength(100);
                enity.Property(b => b.Description).HasMaxLength(150);
                enity.Property(b => b.YearOfPublish).IsRequired();
            });

            modelBuilder.Entity<User>(enity =>
            {
                enity.HasKey(u => u.UserId);
                enity.Property(u => u.UserName).IsRequired().HasMaxLength(50);
                enity.Property(u => u.PasswordHash).IsRequired();
                enity.Property(u => u.RoleId).IsRequired().HasMaxLength(1);

                enity.HasIndex(u => u.UserName).IsUnique();
            });
        }
    }
}