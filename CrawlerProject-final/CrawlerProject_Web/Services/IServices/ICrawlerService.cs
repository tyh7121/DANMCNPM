using CrawlerProject_Web.Models;
using CrawlerProject_Web.Models.DTO;

namespace CrawlerProject_Web.Services.IServices
{
    public interface ICrawlerService
    {
        Task<List<Conference>> GetConferencesAsync(string token);
        Task<Conference> GetConferenceAsync(int id, string token);
        Task<Conference> UpdateConferenceAsync(Conference dto ,string token);
        Task<Conference> DeleteConferenceAsync(int id, string token);
    }
}
