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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        // Inject UserManager 
        public TrainingController(
            ILogger<HomeController> logger,
            e_corpIdentityDbContext e_corpIdentityDbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager) 
        {
            _logger = logger;
            _e_corpIdentityDbContext = e_corpIdentityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bookings = await _e_corpIdentityDbContext.Booking.ToListAsync();
            var coaches = await _e_corpIdentityDbContext.CoachProfile.ToListAsync();
            var sessions = await _e_corpIdentityDbContext.Session.ToListAsync();

            var HomepageView = new Homepage
            {
                HomeBookings = bookings,
                HomeCoaches = coaches,
                HomeSessions = sessions

            };
            return View(HomepageView);
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

        // Create or edit an event (also updates bookings associated with the event)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrEditEvent(SessionAdd model)
        {
            if (ModelState.IsValid)
            {
                Session session;

                // Check for an existing session 
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

                        // Fetch bookings associated with this session
                        var bookings = _e_corpIdentityDbContext.Booking.Where(b => b.SessionID == model.SessionID).ToList();

                        // Update each booking that corresponds to the session
                        foreach (var booking in bookings)
                        {
                            booking.Date = model.Date;
                            booking.Location = model.Location;
                            booking.SessionName = model.Name;
                            booking.CoachID = model.CoachId;

                            // Update CoachName and CoachEmail in the booking
                            var coachProfile = await _e_corpIdentityDbContext.CoachProfile.FirstOrDefaultAsync(cp => cp.CoachID == session.CoachId);
                            string coachName = coachProfile?.Name ?? "Unknown";
                            var coachIdentity = await _userManager.FindByIdAsync(session.CoachId.ToString());
                            string coachEmail = coachIdentity?.Email ?? "Unknown";

                            booking.CoachName = coachName;
                            booking.CoachEmail = coachEmail;
                        }
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



        // Page for creating and editing a coach profile
        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult CreateOrEditCoachProfile()
        {
            // Get the current user's ID
            var userId = _userManager.GetUserId(User);

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

        // Creating and editing a coach profile
        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> CreateOrEditCoachProfile(CreateCoachProfile model)
        {
            if (ModelState.IsValid)
            {
                // Get the current user's ID
                var userId = _userManager.GetUserId(User);

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
        [HttpGet]
        [Authorize]
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
        [HttpGet]
        [Authorize]
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
        [Authorize]
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Event(Guid id)
        {
            var session = await _e_corpIdentityDbContext.Session.FirstOrDefaultAsync(s => s.SessionID == id);

            var bookings = await _e_corpIdentityDbContext.Booking.ToListAsync();

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
                    CoachID = coach.CoachID,
                    SessionID = session.SessionID,
                    Bookings = bookings
                };

                return View(sessionView);
            }

            return RedirectToAction("Events");
        }

        // Delete a Session by ID from the URL and all associated bookings
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var session = await _e_corpIdentityDbContext.Session.FirstOrDefaultAsync(s => s.SessionID == id);

            if (session != null)
            {
                // Get all bookings associated with this session.
                var bookingsForSession = await _e_corpIdentityDbContext.Booking
                    .Where(b => b.SessionID == id)
                    .ToListAsync();

                // Remove those bookings from the database.
                _e_corpIdentityDbContext.Booking.RemoveRange(bookingsForSession);

                // Remove the session itself.
                _e_corpIdentityDbContext.Session.Remove(session);

                // Save changes to the database.
                await _e_corpIdentityDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Events");
        }

        // Make a booking for a session
        public async Task<IActionResult> Book(Guid id)
        {
            var session = await _e_corpIdentityDbContext.Session.FirstOrDefaultAsync(s => s.SessionID == id);

            if (session != null)
            {
                var userId = _userManager.GetUserId(User);

                // Get the coach's name from the CoachProfile model
                var coachProfile = await _e_corpIdentityDbContext.CoachProfile.FirstOrDefaultAsync(cp => cp.CoachID == session.CoachId);
                string coachName = coachProfile?.Name ?? "Unknown";

                // Get the member's email from the identity table
                var memberIdentity = await _userManager.FindByIdAsync(userId);
                string memberEmail = memberIdentity?.Email ?? "Unknown";

                // Assuming that the CoachId corresponds to the User's Id in the identity table to get the email
                var coachIdentity = await _userManager.FindByIdAsync(session.CoachId.ToString());
                string coachEmail = coachIdentity?.Email ?? "Unknown";

                var booking = new Booking
                {
                    BookingID = Guid.NewGuid(),
                    SessionID = session.SessionID,
                    MemberID = userId,
                    Date = session.Date,
                    Location = session.Location,
                    CoachName = coachName, 
                    CoachEmail = coachEmail,
                    CoachID = session.CoachId,
                    SessionName = session.Name,
                    MemberEmail = memberEmail
                };

                _e_corpIdentityDbContext.Booking.Add(booking);
                await _e_corpIdentityDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Events");
        }

        // List all bookings (user logic for this is in the Bookings view)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _e_corpIdentityDbContext.Booking.ToListAsync();

            var bookingsView = new BookingsView
            {
                Bookings = bookings
            };

            return View(bookingsView);
        }

        // Delete a booking by ID from the URL
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var booking = await _e_corpIdentityDbContext.Booking.FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking != null)
            {
                _e_corpIdentityDbContext.Booking.Remove(booking);
                await _e_corpIdentityDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Bookings");
        }

        // List all members that arent in a role for admin or coach
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Members()
        {
            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var viewModel = new UsersWithoutRolesListViewModel();

            foreach (var user in _userManager.Users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (!userRoles.Intersect(allRoles).Any())
                {
                    viewModel.UsersWithoutRoles.Add(new UserWithoutRolesViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email
                    });
                }
            }

            return View(viewModel);
        }


    }
}

