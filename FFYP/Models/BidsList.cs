using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFYP.Models
{
    public class BidsList
    {
        public int BidsListID { get; set; }
        public bool MarkasRead { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
    }
}