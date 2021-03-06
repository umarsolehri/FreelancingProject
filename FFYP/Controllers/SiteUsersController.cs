﻿using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FFYP.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.IO;
using FFYP.ViewModels;
using System;

namespace FFYP.Controllers
{
    [Authorize]
    public class SiteUsersController : BaseSecurityController
    {

        public async Task<ActionResult> Index()
        {
            var user = User.Identity.GetUserId();
            var siteuser = await db.SiteUser.FirstOrDefaultAsync(u => u.UserId == user);
            siteuser.Description = siteuser.Description;
            //var siteUser = db.SiteUser.Include(s => s.Biding);
            return View(siteuser);
        }

        public ActionResult UserNameDisplay()
        {
            var user = User.Identity.GetUserId();
            var siteUser = db.SiteUser.FirstOrDefault(u => u.UserId == user);
            return PartialView("UserNameDisplay", siteUser);
        }
        public ActionResult Edit()
        {
            var user = User.Identity.GetUserId();
            var siteuser = db.SiteUser.FirstOrDefault(u => u.UserId == user);
            int? id = siteuser.SiteUserID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteUser siteUser = db.SiteUser.Find(id);
            if (siteUser == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SiteUserID = new SelectList(db.Biding, "BidingID", "Description", siteUser.SiteUserID);
            return View(siteUser);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SiteUser siteUser, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image == null)
                {
                    db.Entry(siteUser).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    var filename = Path.GetFileName(image.FileName);
                    var extension = Path.GetExtension(filename).ToLower();
                    if (extension == ".png" || extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".gif" || extension == ".gpeg")
                    {
                        if (siteUser.imagepath != null)
                        {
                            var delpath = Path.Combine(Server.MapPath(siteUser.imagepath));
                            System.IO.File.Delete(delpath);
                        }
                        var path = Path.Combine(Server.MapPath("~/Content/images/"), filename);
                        image.SaveAs(path);
                        siteUser.ImageName = filename;
                        siteUser.imagepath = "~/Content/images/" + filename;

                        db.Entry(siteUser).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
            }
            //ViewBag.SiteUserID = new SelectList(db.Biding, "BidingID", "Description", siteUser.SiteUserID);
            return View(siteUser);
        }

        [HttpGet]
        public async Task<ActionResult> JobsPosted()
        {
            var user = User.Identity.GetUserId();
            var siteser = await db.SiteUser.Where(u => u.UserId == user).FirstOrDefaultAsync();
            var myProjects = await db.Project.Where(p => p.SiteUserID == siteser.SiteUserID).ToListAsync();
            return View(myProjects);
        }

        public async Task<ActionResult> MyJobs()
        {
            var user = User.Identity.GetUserId();
            var find = await db.SiteUser.Where(s => s.UserId == user).FirstOrDefaultAsync();
            var job_list = from j in db.Job
                           where j.SellerId == find.SiteUserID
                           join job in db.Project on j.ProId equals job.ProjectID
                           select new JobViewModels
                           {
                               City = job.Location,
                               Email = job.SiteUser.Email,
                               Name = job.SiteUser.FirstName + " " + job.SiteUser.LastName,
                               ProjectStatus = job.ProjectStatus,
                               ContactNumber = job.SiteUser.PhoneNumber,
                               ProjectTitle = job.Title,
                               Id = job.ProjectID
                           };
            return View(job_list);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteUser siteUser = db.SiteUser.Find(id);
            if (siteUser == null)
            {
                return HttpNotFound();
            }
            return View(siteUser);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteUser siteUser = db.SiteUser.Find(id);
            db.SiteUser.Remove(siteUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> MyProPayment()
        {
            var user = User.Identity.GetUserId();
            var findPro = await db.Project.Where(u => u.SiteUser.UserId == user).ToListAsync();
            return View(findPro);
        }


        public async Task<ActionResult> GoToDetails(int id)
        {
            var findpro = await db.Payment.Where(i => i.ProjectID == id).FirstOrDefaultAsync();
            var jobApprove = await db.Biding.Where(p => p.ProjectID == id && p.Status == "Approved").FirstOrDefaultAsync();
            var value = new PaymentViewModels
            {
                Name = jobApprove.SiteUser.FirstName + " " + jobApprove.SiteUser.LastName,
                RecieverID = jobApprove.SiteUserID.ToString(),
                ProjectId = id,
                SenderID = jobApprove.Project.SiteUserID,
                MobileNumber = jobApprove.SiteUser.PhoneNumber,
                IDCard = jobApprove.SiteUser.IDCard
            };
            return View(value);
        }
        [HttpPost]
        public async Task<ActionResult> GoToDetails(PaymentViewModels model,string Amount, string PaymentMethod)
        {
            var save = new Payment
            {
                Amount = model.Amount,
                PayedTo = model.RecieverID,
                PaymentMethod = PaymentMethod,
                ProjectID = model.ProjectId
            };
            db.Payment.Add(save);
            await db.SaveChangesAsync();
            ModelState.AddModelError("", "Payment Succenfully Addedd");
            return View(model);
        }
    }
}