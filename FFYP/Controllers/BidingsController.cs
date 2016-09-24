using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FFYP.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using FFYP.ViewModels;

namespace FFYP.Controllers
{
    [Authorize(Roles = "user")]
    public class BidingsController : BaseSecurityController
    {
        [HttpGet]
        public async Task<ActionResult> SubmitPurposal(BidViewModels model)
        {
            var pro = await db.Project.Where(p => p.ProjectID == model.ProjectID).FirstOrDefaultAsync();

            var BidViewModel = new BidViewModels
            {
                ProjectID = pro.ProjectID,
                ProDescription = HttpUtility.HtmlDecode(pro.Description),
                ProTitle = pro.Title,
            };
            return View(BidViewModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitPurposal(BidViewModels model, string a)
        {

            if (ModelState.IsValid)
            {
                var user = User.Identity.GetUserId();
                var siteUser = await db.SiteUser.Where(s => s.UserId == user).FirstOrDefaultAsync();
                var bid = new Biding
                {
                    BidingDate = DateTime.Now.Date,
                    SiteUserID = siteUser.SiteUserID,
                    Description = HttpUtility.HtmlEncode(model.BidDescription),
                    ProjectID = model.ProjectID,
                    Status = "Pending"
                };

                db.Biding.Add(bid);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "SiteUsers");
            }
            return View(model);
        }
        public async Task<ActionResult> BidingsList(int? id)
        {
            var list = await db.Biding.Where(b => b.ProjectID == id).ToListAsync();
            ViewBag.check = db.Biding.Where(p => p.ProjectID == id).Any(p => p.Status == "Approved");
            return View(list);
        }

        public async Task<ActionResult> Approvebid(int? id)
        {
            var find = await db.Biding.Where(b => b.BidingID == id).FirstOrDefaultAsync();
            find.Status = "Approved";
            db.Entry(find).State = EntityState.Modified;
            await db.SaveChangesAsync();
            var job = new Job
            {
                BuyerId = find.Project.SiteUserID,
                SellerId = find.SiteUserID,
                ProId = find.ProjectID
            };
            db.Job.Add(job);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "SiteUsers");
        }

        // GET: Bidings
        public ActionResult Index()
        {
            var biding = db.Biding.Include(b => b.Project).Include(b => b.SiteUser);
            return View(biding.ToList());
        }

        // GET: Bidings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Biding biding = db.Biding.Find(id);
            if (biding == null)
            {
                return HttpNotFound();
            }
            return View(biding);
        }

        // GET: Bidings/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Project, "ProjectID", "Title");
            ViewBag.BidingID = new SelectList(db.SiteUser, "SiteUserID", "FirstName");
            return View();
        }

        // POST: Bidings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BidingID,BidingDate,Description,Status,ProjectID")] Biding biding)
        {
            if (ModelState.IsValid)
            {
                db.Biding.Add(biding);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Project, "ProjectID", "Title", biding.ProjectID);
            ViewBag.BidingID = new SelectList(db.SiteUser, "SiteUserID", "FirstName", biding.BidingID);
            return View(biding);
        }

        // GET: Bidings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Biding biding = db.Biding.Find(id);
            if (biding == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Project, "ProjectID", "Title", biding.ProjectID);
            ViewBag.BidingID = new SelectList(db.SiteUser, "SiteUserID", "FirstName", biding.BidingID);
            return View(biding);
        }

        // POST: Bidings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BidingID,BidingDate,Description,Status,ProjectID")] Biding biding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(biding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Project, "ProjectID", "Title", biding.ProjectID);
            ViewBag.BidingID = new SelectList(db.SiteUser, "SiteUserID", "FirstName", biding.BidingID);
            return View(biding);
        }

        // GET: Bidings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Biding biding = db.Biding.Find(id);
            if (biding == null)
            {
                return HttpNotFound();
            }
            return View(biding);
        }

        // POST: Bidings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Biding biding = db.Biding.Find(id);
            db.Biding.Remove(biding);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
