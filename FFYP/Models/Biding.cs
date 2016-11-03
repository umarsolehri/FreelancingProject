using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFYP.Models
{
    public class Biding
    {
        public int BidingID { get; set; }
        public DateTime BidingDate { get; set;}
        [AllowHtml]
        public string Description { get; set; }
        public int SiteUserID { get; set; }
        public int BidPrice { get; set; }
        public virtual SiteUser SiteUser { get; set; }
        public string Status { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
    }
}