using FFYP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFYP.ViewModels
{
    public class BidViewModels
    {
        public int BidingID { get; set; }
        public DateTime BidingDate { get; set; }
        [AllowHtml]
        public string BidDescription { get; set; }
        [AllowHtml]
        public string ProDescription { get; set; }
        
        public bool Status { get; set; }
        public string ProTitle { get; set; }
        public string Location { get; set; }
        public string Duration { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
        public int SiteUserID { get; set; }
        public virtual SiteUser SiteUser { get; set; }
    }
}