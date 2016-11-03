using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFYP.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }



        public string Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PayedTo { get; set; }
        public string UserId { get; set; }


        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
    }
}