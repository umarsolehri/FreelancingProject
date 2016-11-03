using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FFYP.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using FFYP.ViewModels;
using PagedList.EntityFramework;

namespace FFYP.Controllers
{
    public static class StringExtensions
    {
        public static string ToSystemString(this IEnumerable<char> source)
        {
            return new string(source.ToArray());
        }
    }
    [Authorize]
    public class ProjectsController : BaseSecurityController
    {
        public async Task<ActionResult> Index(int? page, string search)
        {
            var user = User.Identity.GetUserId();
            var siteUser = await db.SiteUser.Where(u => u.UserId == user).FirstOrDefaultAsync();
            if (search != null && search != "")
            {
                var list = await db.Project.OrderByDescending(o => o.ProjectID).Where(p => p.SiteUserID != siteUser.SiteUserID && p.Title.Contains(search) && p.ProStatus == "Accepted").ToPagedListAsync(page ?? 1, 10);
                return View(list);
            }
            else
            {
                var list = await db.Project.OrderByDescending(o => o.ProjectID).Where(p => p.SiteUserID != siteUser.SiteUserID && p.ProStatus == "Accepted").ToPagedListAsync(page ?? 1, 10);
                return View(list);
            }
        }

        public async Task<ActionResult> ProDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = await db.Project.Where(p => p.ProjectID == id).FirstOrDefaultAsync();
            if (project == null)
            {
                return HttpNotFound();
            }
            else
            {
                project.Description = HttpUtility.HtmlDecode(project.Description);

                int check = 0;
                var user = User.Identity.GetUserId();
                var siteUser = await db.SiteUser.FirstOrDefaultAsync(u => u.UserId == user);
                if (siteUser != null)
                {
                    check = await db.Biding.Where(b => b.SiteUserID == siteUser.SiteUserID && b.ProjectID == id).CountAsync();
                }
                var favstatus = await db.FavPro.Where(u => u.Userid == user).FirstOrDefaultAsync();
                if (favstatus == null)
                {
                    ViewBag.FavStatus = false;
                }
                var bid = new BidViewModels();
                if (check > 0)
                {
                    bid.Status = true;
                }
                else
                {
                    bid.Status = false;
                }
                bid.ProjectID = project.ProjectID;
                bid.ProTitle = project.Title;
                bid.ProDescription = project.Description;
                bid.Location = project.Location;
                bid.Duration = project.Duration;
                return View(bid);
            }
        }

        public ActionResult PostJob()
        {
            //ViewBag.ProjectID = new SelectList(db.ProjectStatus, "ProjectStatusID", "Status");
            //ViewBag.SiteUserID = new SelectList(db.SiteUser, "SiteUserID", "FirstName");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> PostJob(Project project)
        {
            if (ModelState.IsValid)
            {
                //var date = DateTime.Now.Date.ToString("d");
                var user = User.Identity.GetUserId();
                var siteUser = db.SiteUser.FirstOrDefault(u => u.UserId == user);
                project.SiteUserID = siteUser.SiteUserID;
                project.Description = HttpUtility.HtmlEncode(project.Description);
                project.PostedDate = DateTime.Now.Date;
                project.ProStatus = "Pending";
                db.Project.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "SiteUsers");
            }

            //ViewBag.ProjectID = new SelectList(db.ProjectStatus, "ProjectStatusID", "Status", project.ProjectID);
            //ViewBag.SiteUserID = new SelectList(db.SiteUser, "SiteUserID", "FirstName", project.SiteUserID);
            return View(project);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ProjectID = new SelectList(db.ProjectStatus, "ProjectStatusID", "Status", project.ProjectID);
            ViewBag.SiteUserID = new SelectList(db.SiteUser, "SiteUserID", "FirstName", project.SiteUserID);
            return View(project);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ProjectID = new SelectList(db.ProjectStatus, "ProjectStatusID", "Status", project.ProjectID);
            ViewBag.SiteUserID = new SelectList(db.SiteUser, "SiteUserID", "FirstName", project.SiteUserID);
            return View(project);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            db.Project.Remove(project);
            var removeBid = db.Biding.Where(b => b.ProjectID == id).ToList();
            if (removeBid != null)
            {
                foreach (var item in removeBid)
                {
                    db.Biding.Remove(item);
                }
                var removejob = db.Job.Where(j => j.ProId == id).FirstOrDefault();
                if (removejob != null)
                {
                    db.Job.Remove(removejob);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult OtherBids(int? id)
        {
            var list = db.Biding.Where(i => i.ProjectID == id).ToList();
            return PartialView("_PartialOtherBids", list);
        }


        public async Task<ActionResult> AddFav(int id)
        {
            //var find = await db.FavPro.Where(i => i.ProjectID == id).FirstOrDefaultAsync();
            var add = new FavPro
            {
                ProjectID = id,
                Userid = User.Identity.GetUserId()
            };
            db.FavPro.Add(add);
            await db.SaveChangesAsync();
            return RedirectToAction("MyFavList", "Projects");
        }
        public async Task<ActionResult> MyFavList()
        {
            var user = User.Identity.GetUserId();
            var list = await db.FavPro.Where(u => u.Userid == user).ToListAsync();
            return View(list);
        }
        public async Task<ActionResult> RemoveFav(int id)
        {
            var find = await db.FavPro.Where(i => i.FavProID == id).FirstOrDefaultAsync();
            db.FavPro.Remove(find);
            await db.SaveChangesAsync();
            return RedirectToAction("MyFavList", "Projects");
        }

        public async Task<ActionResult> ApprovePro(int id)
        {
            var find = await db.Project.FindAsync(id);
            find.ProStatus = "Accepted";
            db.Entry(find).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }
    }
}
