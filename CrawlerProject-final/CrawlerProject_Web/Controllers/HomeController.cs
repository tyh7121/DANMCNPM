using CrawlerProject_Web.Models;
using CrawlerProject_Web.Models.DTO;
using CrawlerProject_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

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
            List<Conference> list = await _crawlerService.GetConferencesAsync(HttpContext.Session.GetString("JWTToken"));
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateConference(int ConferenceId)
        {
            string token = HttpContext.Session.GetString("JWTToken");
            var response = await _crawlerService.GetConferenceAsync(ConferenceId, token);
            if (response.Id > 0)
            {
                return View(response);
            }
            TempData["err"] = "Error: Conference not found!!";
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateConference(Conference dto)
        {
            string token = HttpContext.Session.GetString("JWTToken");
            var response = await _crawlerService.UpdateConferenceAsync(dto, token);
            if (response.Id > 0)
            {
                TempData["success"] = "Conference Updated successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Error: Something Wrong!";
            return View(dto);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConference(int ConferenceId)
        {
            string token = HttpContext.Session.GetString("JWTToken");
            var response = await _crawlerService.GetConferenceAsync(ConferenceId, token);
            if (response.Id > 0)
            {
                return View(response);
            }
            TempData["err"] = "Error: Conference not found!";
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConference(Conference dto)
        {
            string token = HttpContext.Session.GetString("JWTToken");
            var response = await _crawlerService.DeleteConferenceAsync(dto.Id, token);
            if (response.Id == -1)
            {
                TempData["success"] = "Conference deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Error: Something Wrong!";
            return View(dto);
        }
    }
}
