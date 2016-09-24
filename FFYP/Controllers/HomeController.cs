using FFYP.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
                        Password = "11223344aa"
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
    }
}