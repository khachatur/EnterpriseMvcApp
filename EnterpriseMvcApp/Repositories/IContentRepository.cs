using EnterpriseMvcApp.Models;

namespace EnterpriseMvcApp.Repositories
{
    public interface IContentRepository
    {
        Task<IEnumerable<Content>> GetAllContentsAsync();
        Task<Content> GetContentByIdAsync(Guid id);
        Task AddContentAsync(Content content);
        Task UpdateContentAsync(Content content);
        Task DeleteContentAsync(Guid id);
        Task AddContentVersionAsync(ContentVersion contentVersion);
        Task<IEnumerable<ContentVersion>> GetContentVersionsAsync(Guid contentId);
    }
}
