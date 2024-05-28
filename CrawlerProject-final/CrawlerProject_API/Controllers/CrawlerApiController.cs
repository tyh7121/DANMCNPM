using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrawlerProject_API.Models;
using CrawlerProject_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace CrawlerProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrawlerApiController : Controller
    {
        private readonly IConferenceRepository _context;
        public CrawlerApiController(IConferenceRepository context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Conference>>> GetAll()
        {
            IEnumerable<Conference> conferences = await _context.GetAll();

            return Ok(conferences);
        }
    }
}
