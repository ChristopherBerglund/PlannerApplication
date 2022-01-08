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
        //public int participantsID { get; set; }
        //public List<participant> Participant { get; set; }
        public int userID { get; set; }
        public user? User { get; set; }
    }
}
