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
using System.Security.Claims;

namespace e_corp.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly e_corpIdentityDbContext _e_corpIdentityDbContext;
        private readonly UserManager<IdentityUser> _userManager; // Added UserManager

        // Inject UserManager 
        public TrainingController(
            ILogger<HomeController> logger,
            e_corpIdentityDbContext e_corpIdentityDbContext,
            UserManager<IdentityUser> userManager) 
        {
            _logger = logger;
            _e_corpIdentityDbContext = e_corpIdentityDbContext;
            _userManager = userManager; 
        }

        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public IActionResult CreateEvent()
        {
            // Get all coaches for the dropdown
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

                return RedirectToAction("CreateEvent"); 
            }

            // If we got this far, something failed, redisplay form
            var coaches = await _userManager.GetUsersInRoleAsync("Coach");
            ViewBag.Coaches = new SelectList(coaches, "Id", "UserName");
            return View(model);
        }

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult CreateOrEditCoachProfile()
        {
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user already has a profile
            var existingProfile = _e_corpIdentityDbContext.CoachProfile.FirstOrDefault(cp => cp.CoachID == userId);

            // If they do, return the edit view with the existing profile
            if (existingProfile != null)
            {
                var editModel = new CreateCoachProfile
                {
                    Name = existingProfile.Name,
                    YearsOfExperience = existingProfile.YearsOfExperience,
                    Biography = existingProfile.Biography,
                    ImageUrl = existingProfile.ImageUrl
                };
                return View(editModel);
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> CreateOrEditCoachProfile(CreateCoachProfile model)
        {
            if (ModelState.IsValid)
            {
                // Get the current user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Check if the user already has a profile
                var existingProfile = _e_corpIdentityDbContext.CoachProfile.FirstOrDefault(cp => cp.CoachID == userId);

                // If they do, update the existing profile
                if (existingProfile != null)
                {
                    existingProfile.Name = model.Name;
                    existingProfile.YearsOfExperience = model.YearsOfExperience;
                    existingProfile.Biography = model.Biography;
                    existingProfile.ImageUrl = model.ImageUrl; 
                }

                // If they don't, create a new profile
                else
                {
                    CoachProfile newCoachProfile = new CoachProfile
                    {
                        CoachID = userId,
                        Name = model.Name,
                        YearsOfExperience = model.YearsOfExperience,
                        Biography = model.Biography,
                        ImageUrl = model.ImageUrl 
                    };
                    _e_corpIdentityDbContext.CoachProfile.Add(newCoachProfile);
                }

                await _e_corpIdentityDbContext.SaveChangesAsync();

                return RedirectToAction("CreateOrEditCoachProfile");
            }
            return View(model);
        }

        // List all coaches
        public async Task<IActionResult> Coaches()
        {
            var coaches = _e_corpIdentityDbContext.CoachProfile.ToListAsync();

            var coachesView = new CoachesView
            {
                CoachProfiles = await coaches
            };

            return View(coachesView);
        }

        // View a single coach profile by ID from the URL
        public async Task<IActionResult> Coach(Guid id)
        {
            var coach = await _e_corpIdentityDbContext.CoachProfile.FirstOrDefaultAsync(cp => cp.CoachID == id.ToString());

            if (coach != null)
            {
                var coachView = new CoachView
                {
                    Name = coach.Name,
                    YearsOfExperience = coach.YearsOfExperience,
                    Biography = coach.Biography,
                    ImageUrl = coach.ImageUrl
                };

                return View(coachView);
            }

            return RedirectToAction("Coaches");
        }

        // List all events
        public async Task<IActionResult> Events()
        {
            var sessions = _e_corpIdentityDbContext.Session.ToListAsync();

            var sessionsView = new Sessions
            {
                Events = await sessions
            };

            return View(sessionsView);
        }



    }
}

