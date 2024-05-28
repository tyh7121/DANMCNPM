using CrawlerProject_Web.Models.DTO;
using CrawlerProject_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace CrawlerProject_Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly string crawlerApiUrl;
        public IHttpClientFactory httpClient { get; set; }

        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            crawlerApiUrl = configuration.GetValue<string>("ServiceUrls:CrawlerProjectAPI");
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO obj)
        {
            var client = httpClient.CreateClient("CrawlerProject");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(crawlerApiUrl + "/user/login");
            message.Method = HttpMethod.Post;

            message.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");


            HttpResponseMessage httpResponseMessage = await client.SendAsync(message);
            var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<LoginResponseDTO>(apiContent);

            if (responseContent == null)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = ""
                };
            }

            return responseContent;
        }

        public async Task<UserDTO> RegisterAsync(RegisterationRequestDTO obj)
        {
            var client = httpClient.CreateClient("CrawlerProject");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(crawlerApiUrl + "/user/register");
            message.Method = HttpMethod.Post;

            message.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.SendAsync(message);
            var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<UserDTO>(apiContent);

            return responseContent;
        }
    }
}
