using CrawlerProject_API.Models;

namespace CrawlerProject_API.Repository.IRepository
{
    public interface IConferenceRepository
    {
        Task<List<Conference>> GetAll();
        Task Save();
    }
}
