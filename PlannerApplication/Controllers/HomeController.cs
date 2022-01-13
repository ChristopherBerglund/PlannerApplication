using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using PlannerApplication.Models;
using PlannerApplication.Models.DTO;
using System.Diagnostics;

namespace PlannerApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlannerContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(
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
        [HttpGet]
        public IActionResult Index(string searchString, string a)
        {
            var activities = _context.newactivity.Include("Activity").Include("User").Include("participants").ToList();
            //Radera gamla händelser
            foreach (var activity in activities)
            {
                if (activity.When < DateTime.Now)
                {
                    _context.Remove(activity);
                }
            }
            _context.SaveChanges();

            if (searchString != null && searchString != "Populärt" && searchString != "Senaste")
            {
                activities = _context.newactivity.Where(a => a.Activity.Name == searchString).Include("Activity").Include("User").ToList();
            }
            else if(searchString == "Populärt")
            {
                activities = _context.newactivity.Include("Activity").Include("User").OrderByDescending(x => x.NrOfParticipants).ToList();
            }
            else if (searchString == "Senaste")
            {
                activities = _context.newactivity.Include("Activity").Include("User").OrderBy(x => x.When).ToList();
            }


            int sum = 0;
            foreach (var item in activities)
            {
                sum++;
            }
            ViewBag.Sum = sum;  

            return View(activities);
        }
        [HttpGet]
        public IActionResult GetPost()
        {
            var activites = _context.activity.ToList();
            return View(activites);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostNew(string _activity, string _headline, string _where, DateTime _when, int _min, int _max, int _ageLimit)
        {
            var user = await _userManager.GetUserAsync(User);

            activity act = _context.activity.Where(x => x.Name == _activity).First();
            newactivity newAct = new newactivity()
            {
                activityID = act.activityID,
                Text = _headline,
                Where = _where,
                When = _when,
                userID = user.Id,
                NrOfParticipants = 1,
               
            };

            _context.Add(newAct);
            _context.SaveChanges();
            var myName = _context.planneruser.Where(x => x.userID == user.Id).First();
            participant newPar = new participant()
            {
                Name = myName.firstName,
                userID = user.Id,
                newActivityID = newAct.newActivityID
            };
            _context.participant.Add(newPar);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Join(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var partid = await _context.participant.Where(x => x.userID == user.Id && x.newActivityID == id).FirstOrDefaultAsync();

            if(partid is null)
            {
                var myName = _context.planneruser.Where(x => x.userID == user.Id).First();
                var newParticipant = _context.newactivity.Where(x => x.newActivityID == id).First();
                newParticipant.NrOfParticipants++;
                _context.Update(newParticipant);

                participant parti = new participant()
                {
                    Name = myName.firstName,
                    userID = user.Id,
                    newActivityID = id
                };
                _context.Add(parti);
                _context.SaveChanges();

            }




            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var newParticipant = _context.newactivity.Where(x => x.newActivityID == id).First();
            if(newParticipant.userID == user.Id)
            {
                _context.Remove(newParticipant);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var part = _context.participant.Where(x => x.newActivityID == id).Include("User").ToList();

            return View(part);
        }
        public async Task<IActionResult> myActivites(string searchString)
        {
            var user = await _userManager.GetUserAsync(User);
            var activities = _context.newactivity.Include("Activity").Include("User").Where(x => x.userID == user.Id).ToList();

            //Radera gamla händelser
            foreach (var activity in activities)
            {
                if (activity.When < DateTime.Now)
                {
                    _context.Remove(activity);
                }
            }
            _context.SaveChanges();


            if (searchString != null && searchString != "Populärt" && searchString != "Senaste")
            {
                activities = _context.newactivity.Where(a => a.Activity.Name == searchString).Include("Activity").Include("User").Where(x => x.userID == user.Id).ToList();
            }
            else if (searchString == "Populärt")
            {
                activities = _context.newactivity.Include("Activity").Include("User").OrderByDescending(x => x.NrOfParticipants).Where(x => x.userID == user.Id).ToList();
            }
            else if (searchString == "Senaste")
            {
                activities = _context.newactivity.Include("Activity").Include("User").OrderBy(x => x.When).Where(x => x.userID == user.Id).ToList();
            }


            int sum = 0;
            foreach (var item in activities)
            {
                sum++;
            }
            ViewBag.Sum = sum;

            return View(activities);
        }
        public async Task<IActionResult> showProfile(string? id)
        {
            if(id == null)
            {
                var user = await _userManager.GetUserAsync(User);
                id = user.Id;
            }
            
            var pUser = _context.planneruser.Where(x => x.userID == id).FirstOrDefault();
            return View(pUser);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}









































//public async Task<IActionResult> myProfile()
//{
//    var user = await _userManager.GetUserAsync(User);
//    var pUser = _context.planneruser.Where(x => x.userID == user.Id).Include("picture").FirstOrDefault();
//    var userTags = _context.activity.Where(
//        x => x.activityID == pUser.tagOneID || 
//        x.activityID == pUser.tagTwoID || 
//        x.activityID == pUser.tagThreeID).Select(x => x.Name).ToList();

//    var dto = new userActivity(pUser, userTags);
//    return View(dto);
//}