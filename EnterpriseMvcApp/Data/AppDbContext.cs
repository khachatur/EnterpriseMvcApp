using EnterpriseMvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseMvcApp.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Content> Contents { get; set; }
		public DbSet<ContentVersion> ContentVersions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
				entity.Property(e => e.PasswordHash).IsRequired();
				entity.Property(e => e.CreatedAt).IsRequired();
				entity.Property(e => e.UpdatedAt).IsRequired();
				entity.Property(e => e.Role).IsRequired();
			});

			modelBuilder.Entity<Content>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
				entity.Property(e => e.Body).IsRequired();
				entity.Property(e => e.AuthorId).IsRequired();
				entity.Property(e => e.CreatedAt).IsRequired();
				entity.Property(e => e.UpdatedAt).IsRequired();

				entity.HasOne<User>()
					.WithMany()
					.HasForeignKey(c => c.AuthorId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<ContentVersion>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.ContentId).IsRequired();
				entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
				entity.Property(e => e.Body).IsRequired();
				entity.Property(e => e.CreatedAt).IsRequired();
			});

			// Seed roles
			modelBuilder.Entity<User>().HasData(new User
			{
				Id = Guid.NewGuid(),
				Username = "admin",
				Email = "admin@example.com",
				PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Role = "Admin"
			});
		}
	}
}
