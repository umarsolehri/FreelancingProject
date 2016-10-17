using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFYP.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Duration)]
        public string Duration { get; set; }
        [AllowHtml,Required]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }

        public string Location { get; set; }
        [Display(Name ="Required Skills")]
        public string SkillsRequired { get; set; }
        public string ProjectStatus { get; set; }
        [Display(Name ="Posted by")]
        public int SiteUserID { get; set; }
        public virtual SiteUser SiteUser { get; set; }
        public virtual ICollection<Biding> Biding { get; set; }
    }
}