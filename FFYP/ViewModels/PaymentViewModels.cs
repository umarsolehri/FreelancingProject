using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFYP.ViewModels
{
    public class PaymentViewModels
    {
        public int SenderID { get; set; }
        public string RecieverID { get; set; }
        public string Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string IDCard { get; set; }
    }
}