using EnterpriseMvcApp.Data;
using EnterpriseMvcApp.Models;
using MongoDB.Driver;

namespace EnterpriseMvcApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _mongoCollection;
        private readonly AppDbContext _dbContext;

        public UserRepository(MongoDbContext mongoContext, AppDbContext dbContext)
        {
            _mongoCollection = mongoContext.Users;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _mongoCollection.Find(user => true).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _mongoCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _mongoCollection.Find(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail).FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _mongoCollection.InsertOneAsync(user);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            await _mongoCollection.ReplaceOneAsync(filter, user);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            await _mongoCollection.DeleteOneAsync(filter);

            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
