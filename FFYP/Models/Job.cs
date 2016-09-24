using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FFYP.Models
{
    public class Job
    {
        public int JobID { get; set; }
        //public string Title { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int ProId { get; set; }
    }
}