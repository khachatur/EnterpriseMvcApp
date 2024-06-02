using EnterpriseMvcApp.Models;
using EnterpriseMvcApp.Repositories;

namespace EnterpriseMvcApp.Services
{
    public class ContentService : IContentService
    {
        private readonly IContentRepository _contentRepository;

        public ContentService(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public Task<IEnumerable<Content>> GetAllContentsAsync()
        {
            return _contentRepository.GetAllContentsAsync();
        }

        public Task<Content> GetContentByIdAsync(Guid id)
        {
            return _contentRepository.GetContentByIdAsync(id);
        }

        public Task AddContentAsync(Content content)
        {
            return _contentRepository.AddContentAsync(content);
        }

        public Task UpdateContentAsync(Content content)
        {
            return _contentRepository.UpdateContentAsync(content);
        }

        public Task DeleteContentAsync(Guid id)
        {
            return _contentRepository.DeleteContentAsync(id);
        }

        public Task<IEnumerable<ContentVersion>> GetContentVersionsAsync(Guid contentId)
        {
            return _contentRepository.GetContentVersionsAsync(contentId);
        }
    }
}
