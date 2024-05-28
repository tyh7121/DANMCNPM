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
        public async Task<List<Conference>> GetAllAsync()
        {
            return await _db.Conferences.ToListAsync();
        }

        public async Task<Conference> GetAsync(int id)
        {
            var conference = await _db.Conferences.SingleOrDefaultAsync(x => x.Id == id);
            if (conference == null)
            {
                return new Conference { Id = 0 };
            }
            return conference;
        }

        public async Task RemoveAsync(Conference entity)
        {
            _db.Conferences.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<Conference> UpdateAsync(Conference entity)
        {
            _db.Update(entity);
            await SaveAsync();
            return entity;
        }
    }
}
