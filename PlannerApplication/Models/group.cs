using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Models
{
    public class group
    {
        [Key]
        public int groupID { get; set; }
        public string groupTitle { get; set; }
        public string userID { get; set; }
        public planneruser user { get; set; }
        public List<member> members { get; set; }
        [NotMapped]
        public int Qty { get; set; }
        
    }
}
