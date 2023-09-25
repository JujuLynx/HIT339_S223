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
        public async Task<IActionResult> CreateEvent(Guid? id)
        {
            var coaches = await _userManager.GetUsersInRoleAsync("Coach");
            ViewBag.Coaches = new SelectList(coaches, "Id", "UserName");

            if (id.HasValue)
            {
                var session = await _e_corpIdentityDbContext.Session.FindAsync(id.Value);
                if (session == null)
                    return NotFound();

                var model = new SessionAdd
                {
                    SessionID = session.SessionID,
                    Name = session.Name,
                    Date = session.Date,
                    Location = session.Location,
                    CoachId = session.CoachId
                };

                return View(model);
            }
            return View(new SessionAdd());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrEditEvent(SessionAdd model)
        {
            if (ModelState.IsValid)
            {
                Session session;

                // Check if the SessionID is provided, meaning it's an existing session
                if (model.SessionID != Guid.Empty)
                {
                    session = _e_corpIdentityDbContext.Session.FirstOrDefault(s => s.SessionID == model.SessionID);

                    if (session != null)
                    {
                        // Update the existing session
                        session.Name = model.Name;
                        session.Date = model.Date;
                        session.Location = model.Location;
                        session.CoachId = model.CoachId;
                    }
                    else
                    {
                        return NotFound("Session not found.");
                    }
                }
                else
                {
                    // Create a new session
                    session = new Session
                    {
                        SessionID = Guid.NewGuid(),
                        Name = model.Name,
                        Date = model.Date,
                        Location = model.Location,
                        CoachId = model.CoachId
                    };
                    _e_corpIdentityDbContext.Session.Add(session);
                }

                await _e_corpIdentityDbContext.SaveChangesAsync();

                return RedirectToAction("Events");
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
                    ImageUrl = coach.ImageUrl,
                    CoachID = coach.CoachID
                };

                return View(coachView);
            }

            return RedirectToAction("Coaches");
        }

        [HttpGet]
        // List all events (user-based logic for this is in the Events view)
        public async Task<IActionResult> Events(string id)
        {
            var sessions = await _e_corpIdentityDbContext.Session.ToListAsync();

            var sessionsView = new Sessions
            {
                Events = sessions,
                CoachID = id
            };

            return View(sessionsView);
        }


        // View a single event by ID from the URL
        public async Task<IActionResult> Event(Guid id)
        {
            var session = await _e_corpIdentityDbContext.Session.FirstOrDefaultAsync(s => s.SessionID == id);

            if (session != null)
            {
                var coach = await _e_corpIdentityDbContext.CoachProfile.FirstOrDefaultAsync(c => c.CoachID == session.CoachId);

                if (coach == null)
                {
                    // Handle the case where the coach doesn't exist (optional)
                    return NotFound("Coach not found");
                }

                var sessionView = new SessionView
                {
                    Name = session.Name,
                    Date = session.Date,
                    Location = session.Location,
                    CoachName = coach.Name,
                    SessionID = session.SessionID
                };

                return View(sessionView);
            }

            return RedirectToAction("Events");
        }

        // Delete a Session by ID from the URL
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var session = await _e_corpIdentityDbContext.Session.FirstOrDefaultAsync(s => s.SessionID == id);

            if (session != null)
            {
                _e_corpIdentityDbContext.Session.Remove(session);
                await _e_corpIdentityDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Events");
        }



    }
}

