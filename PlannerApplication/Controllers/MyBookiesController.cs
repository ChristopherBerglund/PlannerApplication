using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using PlannerApplication.Models;

namespace PlannerApplication.Controllers
{
    [Authorize]
    public class MyBookiesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlannerContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public MyBookiesController(
            ILogger<HomeController> logger,
            PlannerContext context,
            SignInManager<IdentityUser> SignInManager,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = SignInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString, string a)
        {
            var user = await _userManager.GetUserAsync(User);
            var allActivites = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).ToList();
            var participants = _context.participant.Where(x => x.userID == user.Id).Select(x => x.newActivityID).ToList();
            List<newactivity> ListOfBookings = new List<newactivity>();
            foreach (var parti in participants)
            {
                foreach (var activity in allActivites)
                {
                    if(activity.newActivityID == parti)
                    {
                        ListOfBookings.Add(activity);
                    }
                }

            }


            //Radera gamla händelser
            foreach (var activity in ListOfBookings)
            {
                if (activity.When < DateTime.Now)
                {
                    activity.bestBeforeDate = true;
                    _context.Update(activity);
                }
            }
            _context.SaveChanges();


            if (searchString != null && searchString != "Populärt" && searchString != "Senaste")
            {
                ListOfBookings = _context.newactivity.Where(a => a.Activity.Name == searchString).Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).Where(x => x.userID == user.Id).ToList();
            }
            else if (searchString == "Populärt")
            {
                ListOfBookings = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).OrderByDescending(x => x.NrOfParticipants).Where(x => x.userID == user.Id).ToList();
            }
            else if (searchString == "Senaste")
            {
                ListOfBookings = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).OrderBy(x => x.When).Where(x => x.userID == user.Id).ToList();
            }


            int sum = 0;
            foreach (var item in ListOfBookings)
            {
                sum++;
            }
            ViewBag.Sum = sum;

            return View(ListOfBookings);
        }


    }
}
