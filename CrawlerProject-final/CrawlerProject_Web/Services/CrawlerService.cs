using CrawlerProject_Web.Models;
using CrawlerProject_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CrawlerProject_Web.Services
{
    public class CrawlerService : ICrawlerService
    {
        private readonly string crawlerApiUrl;
        public IHttpClientFactory httpClient { get; set; }

        public CrawlerService(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            crawlerApiUrl = configuration.GetValue<string>("ServiceUrls:CrawlerProjectAPI");
        }

        public async Task<List<Conference>> GetConferencesAsync(string token)
        {
            var client = httpClient.CreateClient("CrawlerProject");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(crawlerApiUrl + "/CrawlerApi");
            message.Method = HttpMethod.Get;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage httpResponseMessage = await client.SendAsync(message);
            var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<List<Conference>>(apiContent);

            if (responseContent == null )
            {
                return new List<Conference>();
            }
            
            return responseContent;
        }
    }
}
