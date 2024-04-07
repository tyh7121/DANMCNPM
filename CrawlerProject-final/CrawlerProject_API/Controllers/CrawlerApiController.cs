using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrawlerProject_API.Models;

namespace CrawlerProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrawlerApiController : Controller
    {
        private readonly ConferencesContext _context;
        public CrawlerApiController(ConferencesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Conference>>> GetAll([FromQuery]string? search, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<Conference> query = _context.Conferences;
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            IEnumerable<Conference> conferences = await query.ToListAsync();
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                conferences = conferences.Where(h => h.Title.ToLower().Contains(search) || h.Organizer.ToLower().Contains(search));
            }

            return Ok(conferences);
        }
    }
}
