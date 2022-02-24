using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using PlannerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.HelpClasses
{
    public class Helper
    {
        public static void IsItAnOldEvent(List<newactivity> list, PlannerContext _context)
        {
            foreach (var activity in list)
            {
                if (activity.When < DateTime.Now)
                {
                    activity.bestBeforeDate = true;
                    _context.newactivity.Update(activity);
                }
            }
            _context.SaveChanges();
        }



        public static void IsItLessThan2HoursToEventNotfication(List<newactivity> list, PlannerContext _context)
        {
            foreach (var activity in list)
            {
                if (DateTime.Now.AddHours(2) > activity.When)
                {
                    foreach (var user in activity.participants)
                    {
                        if (user.NotificationTwoHours == true && user.TwoHoursNotHasBeenSent == false)
                        {
                            MailSender.TwoHoursMessage(user, activity);
                            user.TwoHoursNotHasBeenSent = true;
                            _context.participant.Update(user);
                        }
                    }
                }
            }
            _context.SaveChanges();
        }


        public static int GetTheDistance(List<newactivity> list, planneruser me)
        {
            int sum = 0;
            foreach (var item in list)
            {
                var Lat = double.Parse(item.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                var Lng = double.Parse(item.Longitude, System.Globalization.CultureInfo.InvariantCulture);
                var Lat1 = double.Parse(me.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                var Lng1 = double.Parse(me.Longtitude, System.Globalization.CultureInfo.InvariantCulture);
                item.Distance = Distance.DistanceTo(Lat, Lng, Lat1, Lng1);
                sum++;
            }
            return sum;
        }

    }
}
