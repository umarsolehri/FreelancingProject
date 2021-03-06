﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FFYP.ViewModels
{
    public class ContactUsViewModels
    {
        [Required(ErrorMessage = "First Name is required."), MaxLength(20), Display(Name = "First Name")]
        [RegularExpression("^([a-zA-Z ]){1,20}$", ErrorMessage = "Name Contains Only Letters with max length 20")]
        public string Name { get; set; }
        [Required, MaxLength(12), Display(Name = "Phone Number")]
        [RegularExpression("^([0-9]){11}$", ErrorMessage = "Numbers Only with length 11")]
        public string PhoneNumber { get; set; }
        [Required, Display(Name = "Email")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,3})$", ErrorMessage = "Email is not a valid e-mail address.")]
        public string Email { get; set; }
        [Required, MaxLength(1000), Display(Name = "Your Message")]
        public string Message { get; set; }
    }
}