using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlannerApplication.Data;
using PlannerApplication.Models;
using PlannerApplication.Models.DTO;

namespace PlannerApplication.Controllers
{
    public class myProfileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlannerContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public myProfileController(
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

        public async Task<IActionResult> showProfile(string? id)
        {
            if (id == null)
            {
                var user = await _userManager.GetUserAsync(User);
                id = user.Id;
            }

           
            var sporter = _context.activity.Select(x => x.Name).ToList();
            ViewBag.Sports = sporter;


            var pUser = _context.planneruser.Where(x => x.userID == id).FirstOrDefault();
            return View(pUser);
        }

        public async Task<IActionResult> Edit(planneruser _user)
        {
            var user = await _userManager.GetUserAsync(User);
            var myProfile = _context.planneruser.Where(x => x.userID == user.Id).FirstOrDefault();
            myProfile.Age = _user.Age;
            myProfile.aboutMe = _user.aboutMe;
            myProfile.tagOneID = _user.tagOneID;
            myProfile.tagTwoID = _user.tagTwoID;
            myProfile.tagThreeID = _user.tagThreeID;
            myProfile.Phone = _user.Phone;

            _context.planneruser.Update(myProfile);
            await _context.SaveChangesAsync();

            return RedirectToAction("showProfile");
        }

        //public async Task<IActionResult> showMyProfile(string? id)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var me = _context.planneruser.Where(x => x.userID == user.Id).FirstOrDefault();
        //    var sports = _context.activity.Select(x => x.Name).ToList();
        //    DTOuserActivity a =  new DTOuserActivity(me, sports);
        //    return View(a);

        //}




    }
}
