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
        public planneruser? User { get; set; }
        //public int participantID { get; set; }
        public ICollection<participant> participants { get; set; }
    }
}
