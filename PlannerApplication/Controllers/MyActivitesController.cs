using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using PlannerApplication.HelpClasses;
using PlannerApplication.Models;

namespace PlannerApplication.Controllers
{
    [Authorize]

    public class MyActivitesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlannerContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public MyActivitesController(
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
            var allActivites = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).Where(x => x.userID == user.Id).ToList();


            if (searchString != null && searchString != "Populärt" && searchString != "Senaste")
            {
                allActivites = _context.newactivity.Where(a => a.Activity.Name == searchString).Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).Where(x => x.userID == user.Id).ToList();
            }
            else if (searchString == "Populärt")
            {
                allActivites = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).Where(x => x.userID == user.Id).OrderByDescending(x => x.NrOfParticipants).Where(x => x.userID == user.Id).ToList();
            }
            else if (searchString == "Senaste")
            {
                allActivites = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).Where(x => x.userID == user.Id).ToList();
            }


            int sum = 0;
            foreach (var item in allActivites)
            {
                sum++;
            }
            ViewBag.Sum = sum;

            return View(allActivites);
        }
        public async Task<IActionResult> Delete(int id, string _reason)
        {
            var user = await _userManager.GetUserAsync(User);
            var thisActivity1 = _context.newactivity.Include(x => x.participants).ThenInclude(x => x.User).Include("Activity").Include("User").ToList();



            var thisActivity = thisActivity1.Where(x => x.newActivityID == id).First();
            if (thisActivity.userID == user.Id)
            {
                foreach (var User in thisActivity.participants)
                {
                    //if(User.User.Email != user.Email)
                    //{
                        MailSender.EventDeletedMessage(User, thisActivity, _reason);
                    //}
                }

                _context.Remove(thisActivity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
