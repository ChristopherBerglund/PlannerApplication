using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PlannerApplication.Models
{
    public class ContentViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Contents { get; set; }
        public byte[] Image { get; set; }
    }
}
