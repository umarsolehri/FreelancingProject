using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFYP.Models
{
    public class JobApproved
    {
        public int JobApprovedID { get; set; }
        public bool MarkasRead { get; set; }
        public int SiteUserID { get; set; }
        public virtual SiteUser SiteUser { get; set; }
    }
}