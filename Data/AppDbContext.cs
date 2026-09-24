using Microsoft.EntityFrameworkCore;

namespace Book.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Boook> Books {get; set;}
        public DbSet<User> Users {get;set;}
        public DbSet<RefreshTokens> RefreshTokens {get;set;}
        public DbSet<Review> Reviews {get;set;}
        public DbSet<ModerationBook> ModerationBooks {get;set;}

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
                enity.Property(u => u.Role).IsRequired().HasMaxLength(20);

                enity.HasIndex(u => u.UserName).IsUnique();
            });

            modelBuilder.Entity<RefreshTokens>(enity =>
            {
                enity.HasKey(e => e.Id);

                // Связь с Users (один пользователь — много токенов)
                enity.HasOne(e => e.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(e => e.IdUser)
                    .OnDelete(DeleteBehavior.Cascade); // При удалении пользователя — удалить его токены

                enity.Property(e => e.RefreshToken)
                    .IsRequired()
                    .HasMaxLength(500);

                enity.Property(e => e.IsRevoked)
                    .IsRequired()
                    .HasDefaultValue(false);
                    
                enity.Property(e => e.ExpiresDate)
                    .IsRequired();

                enity.Property(e => e.CreatedAt)
                    .IsRequired();

                enity.HasIndex(e => e.IdUser);
                enity.HasIndex(e => e.RefreshToken)
                    .IsUnique();  // Токен должен быть уникальным (один токен = один пользователь)
            });

            modelBuilder.Entity<Review>(enity =>
            {
                enity.HasKey(r => r.Id);

                enity.HasIndex(r => r.BookId); // индекс для быстрого поиска отзывов книги

                enity.HasIndex(r => r.UserId); // индекс для быстрого поиска отзывов пользователя

                enity.HasIndex(r => new {r.BookId, r.UserId})
                    .IsUnique(); // уник. индекс - один пользователь = один отзыв на книгу
                
                enity.Property(r => r.Rating)
                    .IsRequired(); // обязательное поле
                
                enity.Property(r => r.Comment)
                    .HasMaxLength(2500); // необязательное поле. максимум 2500
                
                enity.Property(r => r.CreatedAt)
                    .IsRequired();
                
                enity.Property(r => r.UpdatedAt)
                    .IsRequired(false); // заполнится при обновлени пользователя
            });
       
            modelBuilder.Entity<ModerationBook>(enity =>
            {
                enity.HasKey(e => e.BookId);
                enity.HasIndex(e => e.Status);
                enity.Property(e => e.Status).HasConversion<string>();
                enity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}