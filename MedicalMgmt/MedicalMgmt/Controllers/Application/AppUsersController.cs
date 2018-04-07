using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalMgmt.Models;
using PagedList;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace MedicalMgmt.Controllers.Application
{
    [Authorize]
    public class AppUsersController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        //// GET: Users
        //public ActionResult Index()
        //{
        //    return View(db.Users.ToList());
        //}

        // GET: Users
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.UsernameSortParam = String.IsNullOrEmpty(sortOrder) ? "Username_desc" : "";
            ViewBag.FullNameSortParam = sortOrder == "FullName" ? "FullName_desc" : "FullName";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var users = from u in db.AppUsers select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.FullName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Username_desc":
                    users = users.OrderByDescending(u => u.Username);
                    break;
                case "FullName":
                    users = users.OrderBy(u => u.FullName);
                    break;
                case "FullName_desc":
                    users = users.OrderByDescending(u => u.FullName);
                    break;
                default:
                    users = users.OrderBy(u => u.Username);
                    break;
            }

            var pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
            int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: AppUsers/DetailsRedirect/5
        public ActionResult DetailsRedirect(string userId)
        {
            int appUserID = db.AppUsers.Where(u => u.AspNetUserId == userId).Select(u => u.AppUserID).SingleOrDefault();

            return RedirectToAction("Details", new { id = appUserID });
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppUserID,Username,FullName,Telephone,Email,Rg,Cpf,Address")] AppUser appUser)
        {
            appUser.RegisterDate = DateTime.Now;
            appUser.Active = true;
            if (ModelState.IsValid)
            {
                appUser.Username = appUser.Username.ToUpper();
                db.AppUsers.Add(appUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppUserID,AspNetUserId,Username,FullName,Telephone,Email,Rg,Cpf,Address,RegisterDate,Active")] AppUser appUser)
        {
            if (appUser.AspNetUserId != User.Identity.GetUserId())
            {
                return View(appUser);
            }

            if (ModelState.IsValid)
            {
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = appUser.AppUserID });
            }
            return View(appUser);
        }

        //// GET: Users/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    User user = db.Users.Find(id);
        //    db.Users.Remove(user);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
