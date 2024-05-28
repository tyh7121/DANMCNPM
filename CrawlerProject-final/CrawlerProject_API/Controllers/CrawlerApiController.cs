using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrawlerProject_API.Models;
using CrawlerProject_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using CrawlerProject_API.Models.DTO;

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

        [HttpGet(Name = "GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Conference>>> GetAll()
        {
            IEnumerable<Conference> conferences = await _context.GetAllAsync();

            return Ok(conferences);
        }

        [HttpGet("{id:int}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Conference>> Get(int id)
        {
            var conference = await _context.GetAsync(id);
            if (conference.Id == 0)
            {
                return NotFound(conference);
            }

            return Ok(conference);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Conference>> Delete(int id)
        {
            var conference = await _context.GetAsync(id);
            if (conference.Id == 0)
            {
                return NotFound(conference);
            }

            await _context.RemoveAsync(conference);
            return Ok(new Conference { Id = -1 });
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Conference>> Update(int id, [FromBody] Conference updateDTO)
        {
            var conference = await _context.GetAsync(id);
            if (conference.Id == 0)
            {
                return NotFound(conference);
            }

            conference.Date = updateDTO.Date;
            conference.Title = updateDTO.Title;
            conference.Country = updateDTO.Country;
            conference.Url = updateDTO.Url;
            conference.EventStatus = updateDTO.EventStatus;
            conference.Organizer = updateDTO.Organizer;
            conference.Deadline = updateDTO.Deadline;
            conference.StartDate = updateDTO.StartDate;
            conference.EndDate = updateDTO.EndDate;
            conference.Secretary = updateDTO.Secretary;
            conference.InquiryEmail = updateDTO.InquiryEmail;
            conference.RegistrationUrl = updateDTO.RegistrationUrl;

            await _context.UpdateAsync(conference);
            return Ok(conference);
        }
    }
}
