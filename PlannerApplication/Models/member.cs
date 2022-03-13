using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Models
{
    public class member
    {
        [Key]
        public int memberID { get; set; }
        [ForeignKey("groupID")]
        public int groupID { get; set; }
        public group group { get; set; }
        [ForeignKey("userID")]
        public string userID { get; set; }
        public planneruser User { get; set; }
        public bool isMember { get; set; } = false;
    }
}
