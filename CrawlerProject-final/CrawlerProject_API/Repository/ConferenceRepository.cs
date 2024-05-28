using CrawlerProject_API.Data;
using CrawlerProject_API.Models;
using CrawlerProject_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CrawlerProject_API.Repository
{
    public class ConferenceRepository : IConferenceRepository
    {
        private readonly ApplicationDbContext _db;
        public ConferenceRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<Conference>> GetAll()
        {
            return await _db.Conferences.ToListAsync();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
