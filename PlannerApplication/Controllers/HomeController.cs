using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using PlannerApplication.HelpClasses;
using PlannerApplication.Models;
using PlannerApplication.Models.DTO;
using System.Diagnostics;


namespace PlannerApplication.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> IndexAsync(string searchString, string a)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.User = user.Id;
            var me = _context.planneruser.Where(x => x.userID == user.Id).FirstOrDefault();
            ViewBag.Age = me.Age;

            var activities = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).OrderByDescending(x => x.When).ToList();
           
            //int sum = Helper.GetTheDistance(activities, me);

            if (searchString != null && searchString != "Populärt" && searchString != "Senaste" && searchString != "Barn" && searchString != "Närmast mig")
            {
                activities = _context.newactivity.Where(a => a.Activity.Name.StartsWith(searchString)).Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).OrderByDescending(x => x.When).ToList();
            }
            else if(searchString == "Populärt")
            {
                activities = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).OrderByDescending(x => x.NrOfParticipants).ToList();
            }
            else if (searchString == "Senaste")
            {
                activities = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).OrderBy(x => x.When).ToList();
            }
            else if (searchString == "Barn")
            {
                activities = _context.newactivity.Where(a => a.isForKids == true).Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).OrderByDescending(x => x.When).ToList();
            }

            else if (searchString == "Närmast mig")
            {
                activities = _context.newactivity.Include("Activity").Include("User").Include(x => x.participants).ThenInclude(x => x.User).OrderBy(x => x.Distance).ToList();
            }

            Helper.IsItAnOldEvent(activities, _context);
            Helper.IsItLessThan2HoursToEventNotfication(activities, _context);
            ViewBag.Sum = Helper.GetTheDistance(activities, me);


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
        public async Task<IActionResult> PostNew(string _activity, string _headline, DateTime _when, int _min, int _max, int _ageLimit, bool _kids, bool _Involved, bool _notification, string _lat, string _lng, string _address)
        {
            var user = await _userManager.GetUserAsync(User);

            activity act = _context.activity.Where(x => x.Name == _activity).First();
            newactivity newAct = new newactivity()
            {
                activityID = act.activityID,
                Text = _headline,
                Where = _address,
                When = _when,
                userID = user.Id, 
                NrOfParticipants = _Involved ? 1 : 0,
                nrOfMaxParticipants = _max,
                nrOfMinParticipants = _min,
                ageLimit = _ageLimit,
                isForKids = _kids,
                Longitude = _lng,
                Latitude = _lat,

            };
            await _context.AddAsync(newAct);
            await _context.SaveChangesAsync();

            if(_Involved == true)
            {
                var myName = _context.planneruser.Where(x => x.userID == user.Id).First();
                participant newPar = new participant()
                {
                    Name = myName.firstName,
                    userID = user.Id,
                    newActivityID = newAct.newActivityID,
                    NotificationMinimum = _notification
                };
                await _context.participant.AddAsync(newPar);
                await _context.SaveChangesAsync();
            }
           
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Join(int id, bool _canceled, bool _minimum, bool _twohours)
        {
            var user = await _userManager.GetUserAsync(User);
            var partid = await _context.participant.Where(x => x.userID == user.Id && x.newActivityID == id).FirstOrDefaultAsync();
            var myName = _context.planneruser.Where(x => x.userID == user.Id).First();
            var newParticipant = _context.newactivity.Where(x => x.newActivityID == id).First();

            if (partid is null)
            {
                participant parti = new participant()
                {
                    Name = myName.firstName,
                    userID = user.Id,
                    newActivityID = id,
                    NotificationCanceled = _canceled,
                    NotificationMinimum = _minimum,
                    NotificationTwoHours = _twohours
                };

                newParticipant.NrOfParticipants++;
                _context.newactivity.Update(newParticipant);
                await _context.participant.AddAsync(parti);
                await _context.SaveChangesAsync();
            }

            if(newParticipant.NrOfParticipants == newParticipant.nrOfMinParticipants)
            {
                foreach (var User in newParticipant.participants)
                {
                    if (User.NotificationMinimum == true)
                    {
                        MailSender.MinimumMessage(User, newParticipant);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> UnJoin(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var partid = await _context.participant.Where(x => x.userID == user.Id && x.newActivityID == id).FirstOrDefaultAsync();
            
            if (partid is not null)
            {
                var thisActivity = _context.newactivity.Where(x => x.newActivityID == id).FirstOrDefault();
                thisActivity.NrOfParticipants--;
                _context.newactivity.Update(thisActivity);
                _context.participant.Remove(partid);
                _context.SaveChangesAsync().Wait();
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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}





































