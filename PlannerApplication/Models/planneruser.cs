using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Models
{
    public class planneruser
    {
        [Key]
        public string userID { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Age { get; set; }

        public int pictureID { get; set; } = 6;
        public picture picture { get; set; } 
        public string aboutMe { get; set; } = "Information om mig kommer inom kort...";
        public int? tagOneID { get; set; } = 1;
        //public activity? TagOne { get; set; }
        public int? tagTwoID { get; set; } = 2;
        //public activity? TagTwo { get; set; }
        public int? tagThreeID { get; set; } = 3;
        //public activity? TagThree { get; set; }
        public string standardPicture { get; set; } = "/img/userIcon.png´";
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        //public string ProfilePicture { get; set; }
        //[ForeignKey("ImageID")]
        //public int ImageID { get; set; }
        //public imagemodel ImageModel { get; set; }


    }
}
