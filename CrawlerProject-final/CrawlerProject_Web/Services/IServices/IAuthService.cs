using CrawlerProject_Web.Models.DTO;

namespace CrawlerProject_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO obj);
        Task<UserDTO> RegisterAsync(RegisterationRequestDTO obj);
    }
}
