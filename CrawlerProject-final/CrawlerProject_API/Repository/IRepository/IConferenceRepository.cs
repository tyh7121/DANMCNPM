using CrawlerProject_API.Models;

namespace CrawlerProject_API.Repository.IRepository
{
    public interface IConferenceRepository
    {
        Task<Conference> GetAsync(int id);
        Task<List<Conference>> GetAllAsync();
        Task<Conference> UpdateAsync(Conference entity);
        Task RemoveAsync(Conference entity);
        Task SaveAsync();
    }
}
