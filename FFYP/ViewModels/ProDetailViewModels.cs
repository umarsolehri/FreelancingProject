using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFYP.ViewModels
{
    public class ProDetailViewModels
    {
        public int ProId { get; set; }
        public string ProTitle { get; set; }
        [AllowHtml]
        public string ProDesc { get; set; }
        public string Location { get; set; }
        public string Duration { get; set; }
        public string BidStatus { get; set; }
    }
}