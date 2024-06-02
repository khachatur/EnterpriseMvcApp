using EnterpriseMvcApp.Models;

namespace EnterpriseMvcApp.Services
{
    public interface IContentService
    {
        Task<IEnumerable<Content>> GetAllContentsAsync();
        Task<Content> GetContentByIdAsync(Guid id);
        Task AddContentAsync(Content content);
        Task UpdateContentAsync(Content content);
        Task DeleteContentAsync(Guid id);
        Task<IEnumerable<ContentVersion>> GetContentVersionsAsync(Guid contentId);
    }
}
