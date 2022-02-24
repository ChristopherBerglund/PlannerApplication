using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Models
{
    public class participant
    {
        [Key]
        public int participantID { get; set; }
        public string Name { get; set; }
        [ForeignKey("newActivityID")] //
        public int newActivityID { get; set; }
        public newactivity newactivity { get; set; }
        [ForeignKey("userID")] 
        public string userID { get; set; } = string.Empty;
        public planneruser? User { get; set; }
        public bool NotificationCanceled { get; set; } = false;
        public bool NotificationMinimum { get; set; } = false;
        public bool NotificationTwoHours { get; set; } = false;
        public bool TwoHoursNotHasBeenSent { get; set; } = false;
    }
}
