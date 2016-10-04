using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFYP.ViewModels
{
    public class BidListViewModels
    {
        public int BidsListID { get; set; }
        public bool MarkasRead { get; set; }
        public string MadeBy { get; set; }
        public string Title { get; set; }
    }
    public class JobListViewModels
    {
        public int JobApprovedID { get; set; }
        public bool MarkasRead { get; set; }
        public string ApprovedBy { get; set; }
        public string Title { get; set; }
    }
}