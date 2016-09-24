using FFYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFYP.ViewModels
{
    public class JobViewModels
    {
        /// <summary>
        /// Project From
        /// </summary>
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string City { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectStatus { get; set; }
        public int Id { get; set; }
    }
}