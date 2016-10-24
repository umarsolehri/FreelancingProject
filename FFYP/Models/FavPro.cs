using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFYP.Models
{
    public class FavPro
    {
        public int FavProID { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
        public string Userid { get; set; }
    }
}