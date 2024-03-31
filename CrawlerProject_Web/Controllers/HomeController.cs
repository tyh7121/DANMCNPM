using CrawlerProject_Web.Models;
using CrawlerProject_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CrawlerProject_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICrawlerService _crawlerService;

        public HomeController(ICrawlerService crawlerService)
        {
            _crawlerService = crawlerService;
        }

        public async Task<IActionResult> Index()
        {
            List<Conference> list = await _crawlerService.GetConferencesAsync();
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
