using CrawlerProject_API.Models;
using CrawlerProject_API.Models.DTO;
using CrawlerProject_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CrawlerProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);

            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);
        }

        [HttpPost("register")]
        public async Task<ActionResult<LocalUser>> Register([FromBody] RegisterationRequestDTO model)
        {
            bool isUnique = _userRepository.IsUniqueUser(model.UserName);
            if (!isUnique)
            {
                return BadRequest(new LocalUser());
            }

            var user = await _userRepository.Register(model);
            return Ok(user);
        }
    }
}

