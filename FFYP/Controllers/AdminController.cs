using FFYP.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FFYP.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : BaseSecurityController
    {
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            var prolists = await db.Project.OrderByDescending(p => p.ProjectID).ToListAsync();
            return View(prolists);
        }

        public async Task<ActionResult> UserLists()
        {
            var user = from u in db.Users
                       from ur in u.Roles
                       join r in db.Roles on ur.RoleId equals r.Id
                       select new UserViewModels
                       {
                           Id = u.Id,
                           Email = u.UserName,
                           Name = u.FirstName + " " + u.LastName,
                           Role = r.Name,
                       };

            var list = await user.OrderBy(u => u.Name).ToListAsync();
            return View(list);
        }

        public async Task<ActionResult> Deleteuser(string id)
        {
            var user = await db.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            var siteUser = await db.SiteUser.Where(u => u.UserId == user.Id).FirstOrDefaultAsync();
            var job = await db.Job.Where(u => u.SellerId == siteUser.SiteUserID).ToListAsync();
            if (job != null)
            {
                foreach (var item in job)
                {
                    db.Job.Remove(item);
                }
            }
            if (siteUser.imagepath != null)
            {
                var path = Path.Combine(Server.MapPath(siteUser.imagepath));
                System.IO.File.Delete(siteUser.ImageName);
            }
            db.Users.Remove(user);
            db.SiteUser.Remove(siteUser);
            await db.SaveChangesAsync();
            return View();
        }
    }
}