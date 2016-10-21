using FFYP.Models;
using FFYP.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;

namespace FFYP.Controllers
{
    [Authorize]
    public class HomeController : BaseSecurityController
    {
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var roles = ApplicationRoleManager.Create(HttpContext.GetOwinContext());

            if (!await roles.RoleExistsAsync("admin"))
            {
                await roles.CreateAsync(new IdentityRole { Name = "admin" });
            }

            if (!await roles.RoleExistsAsync("user"))
            {
                await roles.CreateAsync(new IdentityRole { Name = "user" });
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactUsViewModels contact)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p> <p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("demomailpro@gmail.com"));
                message.From = new MailAddress(contact.Email);
                message.Subject = "Message From Visitor";
                message.Body = string.Format(body, contact.Name, contact.Email, contact.Message);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "demomailpro@gmail.com",
                        Password = "Az@123456"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Index");
                }
            }
            return View(contact);
        }

        public ActionResult NotificationShow()
        {
            var user = User.Identity.GetUserId();
            var bidslist = db.BidsList.Where(i => i.Project.SiteUser.UserId == user && i.MarkasRead == false).ToList().Count();
            var bidspprove = db.JobApproved.Where(i => i.SiteUser.UserId == user && i.MarkasRead == false).ToList().Count();
            ViewBag.Count = false;
            if (bidslist > 0 || bidspprove > 0)
            {
                ViewBag.Count = true;
            }
            else
            {
                ViewBag.Count = false;
            }
            return PartialView("_PartialNotificationShow");
        }
        public ActionResult Notifications()
        {
            var user = User.Identity.GetUserId();
            var getUser = new UserID
            {
                UserId = user
            };
            return View(getUser);
        }

        public ActionResult BidsList(string id)
        {
            //var li = new List<Biding>();
            var list = db.BidsList.Where(i => i.Project.SiteUser.UserId == id && i.MarkasRead == false).ToList();
            //foreach (var item in list)
            //{
            //    li = db.Biding.Where(p => p.ProjectID == item.ProjectID).ToList();
            //}
            ////where a.Project.SiteUser.UserId == id &&
            //var l = from a in db.BidsList
            //        where a.Project.SiteUser.UserId == id && a.MarkasRead == false
            //        join j in db.Biding on a.ProjectID equals j.ProjectID
            //        select new BidListViewModels
            //        {
            //            BidsListID = a.BidsListID,
            //            MadeBy = j.SiteUser.FirstName + " " + j.SiteUser.LastName,
            //            MarkasRead = a.MarkasRead,
            //            Title = a.Project.Title
            //        };
            return PartialView("_PartialBidsList", list);
        }
        public ActionResult JobsList(string id)
        {
            //var list = db.JobApproved.Where(i => i.SiteUser.UserId == id && i.MarkasRead == false).ToList();
            var l = from a in db.JobApproved
                    where a.SiteUser.UserId == id && a.MarkasRead == false
                    join j in db.Biding on a.SiteUserID equals j.SiteUserID
                    select new JobListViewModels
                    {
                        JobApprovedID = a.JobApprovedID,
                        ApprovedBy = j.Project.SiteUser.FirstName + " " + j.Project.SiteUser.LastName,
                        MarkasRead = a.MarkasRead,
                        Title = j.Project.Title
                    };
            return PartialView("_PartialJobsList", l);
        }
        //[HttpPost]
        public ActionResult BidsMarkAsRead(int id)
        {
            var find = db.BidsList.Where(f => f.BidsListID == id).FirstOrDefault();
            find.MarkasRead = true;
            db.Entry(find).State = EntityState.Modified;
            db.SaveChangesAsync();
            return RedirectToAction("Notifications");
        }
        //[HttpPost]
        public ActionResult JobsMarkAsRead(int id)
        {
            var find = db.JobApproved.Where(f => f.JobApprovedID == id).FirstOrDefault();
            find.MarkasRead = true;
            db.Entry(find).State = EntityState.Modified;
            db.SaveChangesAsync();
            return RedirectToAction("Notifications");
        }
    }
}