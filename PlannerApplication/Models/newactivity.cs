using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Models
{
    public class newactivity
    {
        public int newActivityID { get; set; }
        public int activityID { get; set; }
        public activity? Activity { get; set; }
        public string Where { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime When { get; set; }
        public int NrOfParticipants { get; set; }
        public string userID { get; set; } = string.Empty;
        public planneruser User { get; set; }
        public int? nrOfMaxParticipants { get; set; } = 100;
        public int? nrOfMinParticipants { get; set; } = 0;
        public int? ageLimit { get; set; } = 0;
        public bool bestBeforeDate { get; set; } = false;
        public bool isForKids { get; set; } = false;
        public bool isHidden { get; set; } = false;
        public List<participant> participants { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double Distance { get; set; }
        //public bool Public { get; set; } = true;
        //public int groupID { get; set; }
        //public group group { get; set; }
    }
}
