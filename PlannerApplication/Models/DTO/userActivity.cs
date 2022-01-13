using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Models.DTO
{
    public class userActivity
    {
        public userActivity(planneruser pla, List<string> act)
        {
            this.firstName = pla.firstName;
            this.lastName = pla.lastName;
            this.interest1 = act[0];
            this.interest2 = act[1];
            this.interest3 = act[2];
            this.picUrl = pla.picture.Url;
            this.Age = pla.Age;
            this.Phone = pla.Phone;
            this.Mail = pla.Email;
            this.aboutMe = pla.aboutMe;
        }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? interest1 { get; set; }
        public string? interest2 { get; set; }
        public string? interest3 { get; set; }
        public string? picUrl { get; set; }
        public int? Age { get; set; }
        public string? Phone { get; set; }
        public string? Mail { get; set; }
        public string? aboutMe { get; set; }

    }

    
}
