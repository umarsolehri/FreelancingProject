using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFYP.Models
{
    public class SiteUser
    {
        public int SiteUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name ="Name")]
        public string FullName { get { return FirstName + " " + LastName; } }

        [DataType(DataType.Text)]
        public string Country { get; set; }
        [DataType(DataType.Text)]
        public string City { get; set; }

        public string Degree { get; set; }
        public string Experiance { get; set; }

        [DataType(DataType.Text)]
        public string CareerLevel { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.MultilineText)]
        public string Adddress { get; set; }
        public string IDCard { get; set; }
        public string ImageName { get; set; }
        public string imagepath { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public virtual ICollection<Biding> Biding { get; set; }
    }
}