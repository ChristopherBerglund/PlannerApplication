using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Picture { get; set; } = "/img/userIcon.png";

    }
}
