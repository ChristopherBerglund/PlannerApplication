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
        public int newActivityID { get; set; }
        public newactivity newactivity { get; set; }
        public string userID { get; set; }
        public planneruser User { get; set; }
    }
}
