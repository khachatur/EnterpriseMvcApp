using EnterpriseMvcApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace EnterpriseMvcApp.Data
{
	public class DataSeeder
	{
		private readonly IMongoCollection<User> _usersCollection;
		private readonly ILogger<DataSeeder> _logger;

		public DataSeeder(MongoDbContext dbContext, ILogger<DataSeeder> logger)
		{
			_usersCollection = dbContext.Users;
			_logger = logger;
		}

		public async Task SeedAsync()
		{
			_logger.LogInformation("Seeding MongoDB database...");

			// Check if the admin user already exists
			var adminUser = await _usersCollection.Find(u => u.Email == "admin@example.com").FirstOrDefaultAsync();

			if (adminUser == null)
			{
				// Create the admin user
				var newAdminUser = new User
				{
					Id = Guid.NewGuid(),
					Username = "admin",
					Email = "admin@example.com",
					PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow,
					Role = "Admin"
				};

				await _usersCollection.InsertOneAsync(newAdminUser);
				_logger.LogInformation("Admin user created with ID: {UserId}", newAdminUser.Id);
			}
			else
			{
				_logger.LogInformation("Admin user already exists.");
			}

			_logger.LogInformation("MongoDB database seeding completed.");
		}
	}
}
