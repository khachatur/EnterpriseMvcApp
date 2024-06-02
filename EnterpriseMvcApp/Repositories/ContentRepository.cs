using EnterpriseMvcApp.Data;
using EnterpriseMvcApp.Models;
using MongoDB.Driver;

namespace EnterpriseMvcApp.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly IMongoCollection<Content> _mongoCollection;
        private readonly IMongoCollection<ContentVersion> _mongoVersionCollection;
        private readonly AppDbContext _dbContext;

        public ContentRepository(MongoDbContext mongoContext, AppDbContext dbContext)
        {
            _mongoCollection = mongoContext.Contents;
            _mongoVersionCollection = mongoContext.ContentVersions;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Content>> GetAllContentsAsync()
        {
            return await _mongoCollection.Find(content => true).ToListAsync();
        }

        public async Task<Content> GetContentByIdAsync(Guid id)
        {
            return await _mongoCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddContentAsync(Content content)
        {
            await _mongoCollection.InsertOneAsync(content);
            await _dbContext.Contents.AddAsync(content);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateContentAsync(Content content)
        {
            var filter = Builders<Content>.Filter.Eq(c => c.Id, content.Id);
            await _mongoCollection.ReplaceOneAsync(filter, content);

            _dbContext.Contents.Update(content);
            await _dbContext.SaveChangesAsync();

            // Save content version
            var contentVersion = new ContentVersion
            {
                Id = Guid.NewGuid(),
                ContentId = content.Id,
                Title = content.Title,
                Body = content.Body,
                CreatedAt = DateTime.UtcNow
            };
            await AddContentVersionAsync(contentVersion);
        }

        public async Task DeleteContentAsync(Guid id)
        {
            var filter = Builders<Content>.Filter.Eq(c => c.Id, id);
            await _mongoCollection.DeleteOneAsync(filter);

            var content = await _dbContext.Contents.FindAsync(id);
            if (content != null)
            {
                _dbContext.Contents.Remove(content);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddContentVersionAsync(ContentVersion contentVersion)
        {
            await _mongoVersionCollection.InsertOneAsync(contentVersion);
            await _dbContext.ContentVersions.AddAsync(contentVersion);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContentVersion>> GetContentVersionsAsync(Guid contentId)
        {
            return await _mongoVersionCollection.Find(v => v.ContentId == contentId).ToListAsync();
        }
    }
}
