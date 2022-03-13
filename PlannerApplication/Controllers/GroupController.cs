using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using PlannerApplication.HelpClasses;
using PlannerApplication.Models;

namespace PlannerApplication.Controllers
{
    public class GroupController : Controller
    {
        private readonly PlannerContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public GroupController(
            PlannerContext context,
            SignInManager<IdentityUser> SignInManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = SignInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            ViewBag.User = user.Id;

            var allGroups = await _context.group.Include("user").Include(x => x.members).ThenInclude(x => x.User).ToListAsync();
            foreach (var item in allGroups)
            {
                item.Qty = item.members.Where(x => x.isMember == true).Count();
            }
            return View(allGroups);
        }

        public async Task<IActionResult> JoinGroup(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.User = user.Id;

            member newMember = new member()
            {
                groupID = id,
                userID = user.Id,
            };
            
            _context.member.Add(newMember);
            await _context.SaveChangesAsync();


            var ThisGroup = _context.group.Where(x => x.groupID == id).Include(x => x.user).FirstOrDefault();
            var ThisUser = _context.planneruser.Where(x => x.userID == user.Id).FirstOrDefault();
            MailSender.WantsToJoinYourGroupNotifikation(ThisGroup, ThisUser);


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create(string _title)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.User = user.Id;

            group newGroup = new group()
            {
                groupTitle = _title,
                userID = user.Id
            };

            _context.group.Add(newGroup);
            await _context.SaveChangesAsync(); 

            member newMember = new member()
            {
                groupID = newGroup.groupID,
                userID = user.Id,
                isMember = true
            };

            _context.member.Add(newMember);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GroupHandler()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.myId = user.Id;
            var myGroup = _context.group.Include("user").Where(x => x.user.userID == user.Id).Include(x => x.members).ThenInclude(x => x.User).FirstOrDefault();
        
            return View(myGroup);
        }

        public async Task<IActionResult> AcceptMember(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.myId = user.Id;
            var myGroup = _context.group.Include("user").Where(x => x.user.userID == user.Id).Include(x => x.members).ThenInclude(x => x.User).FirstOrDefault();
            foreach (var item in myGroup.members)
            {
                if(item.User.userID == id)
                {
                    item.isMember = true;
                    _context.member.Update(item);
                    MailSender.YouWhereAcceptedToGroup(myGroup, item.User);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("GroupHandler");
        }

        public async Task<IActionResult> DenieMember(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.myId = user.Id;
            var myGroup = _context.group.Include("user").Where(x => x.user.userID == user.Id).Include(x => x.members).ThenInclude(x => x.User).FirstOrDefault();
            foreach (var item in myGroup.members)
            {
                if (item.User.userID == id)
                {
                    _context.member.Remove(item);
                    MailSender.YouWhereDeniedEntranceToGroup(myGroup, item.User);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("GroupHandler");
        }




    }
}
