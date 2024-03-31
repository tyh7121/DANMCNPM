using CrawlerProject_Web.Models;

namespace CrawlerProject_Web.Services.IServices
{
    public interface ICrawlerService
    {
        Task<List<Conference>> GetConferencesAsync();
    }
}
