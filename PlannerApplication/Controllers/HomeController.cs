using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using PlannerApplication.Models;
using System.Diagnostics;

namespace PlannerApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlannerContext _context;
        public HomeController(ILogger<HomeController> logger, PlannerContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(string searchString)
        {
            var activities = _context.newactivity.Include("Activity").Include("User").ToList();
            if (searchString != null)
            {
                activities = _context.newactivity.Where(a => a.Activity.Name == searchString).Include("Activity").Include("User").ToList();
            }
            return View(activities);
        }
        [HttpGet]
        public IActionResult GetPost()
        {
            var activites = _context.activity.ToList();
            return View(activites);
        }
        [HttpPost]
        public IActionResult PostNew(string _activity, string _headline, string _where, DateTime _when, int _owner)
        {
            //
            activity act = _context.activity.Where(x => x.Name == _activity).First();
            newactivity newAct = new newactivity()
            {
                Activity = act,
                Text = _headline,
                Where = _where,
                When = _when,
                userID = _owner
            };
            _context.Add(newAct);
            _context.SaveChanges();

            return RedirectToAction("Index");
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






