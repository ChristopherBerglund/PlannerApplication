using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Models
{
    public class imagemodel
    {
        [Key]
        public int ImageID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }


    }
}
