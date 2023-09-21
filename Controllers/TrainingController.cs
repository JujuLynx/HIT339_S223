using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using e_corp.Models;
using e_corp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

namespace e_corp.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly e_corpIdentityDbContext _e_corpIdentityDbContext;
        private readonly UserManager<IdentityUser> _userManager; // Added UserManager

        // Inject UserManager into your controller's constructor
        public TrainingController(
            ILogger<HomeController> logger,
            e_corpIdentityDbContext e_corpIdentityDbContext,
            UserManager<IdentityUser> userManager) // Injection here
        {
            _logger = logger;
            _e_corpIdentityDbContext = e_corpIdentityDbContext;
            _userManager = userManager; // Assign to the field
        }

        public IActionResult Index()
        {
            return View();
        }

        // CREATE ITEM PAGE
        [Authorize]
        [HttpGet]
        public IActionResult CreateEvent()
        {
            var coaches = _userManager.GetUsersInRoleAsync("Coach").Result;
            ViewBag.Coaches = new SelectList(coaches, "Id", "UserName");
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent(SessionAdd model)
        {
            if (ModelState.IsValid)
            {
                // Map SessionAdd model to Session model
                Session newSession = new Session
                {
                    SessionID = Guid.NewGuid(),
                    Name = model.Name,
                    Date = model.Date,
                    Location = model.Location,
                    CoachId = model.CoachId
                };

                // Save to database
                _e_corpIdentityDbContext.Session.Add(newSession);
                await _e_corpIdentityDbContext.SaveChangesAsync();

                return RedirectToAction("CreateEvent"); // Redirect to some view (like the list of sessions) after saving.
            }

            // If we got this far, something failed, redisplay form
            var coaches = await _userManager.GetUsersInRoleAsync("Coach");
            ViewBag.Coaches = new SelectList(coaches, "Id", "UserName");
            return View(model);
        }


    }
}

