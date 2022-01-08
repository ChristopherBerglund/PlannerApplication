using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Models
{
    public class forum
    {
        public int forumID { get; set; }
        public int activityID { get; set; }
        public activity? Activity { get; set; }
        public string Test { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.Now;
        public int userID { get; set; }
        public user? User { get; set; }
    }
}
