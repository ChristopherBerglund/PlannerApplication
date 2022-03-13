using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.HelpClasses
{
    public class GroupHelper
    {
        public static bool IsThereNotificaton(PlannerContext _context, string id)
        {
            var mygroup = _context.group.Include(x => x.members).Include(x => x.user).Where(x => x.user.userID == id).FirstOrDefault();
            if(mygroup != null)
            {
                foreach (var item in mygroup.members)
                {
                    if (item.isMember == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
