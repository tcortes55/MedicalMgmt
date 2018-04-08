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

        private static string pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
        int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);

        //// GET: Users
        //public ActionResult Index()
        //{
        //    return View(db.Users.ToList());
        //}

        // GET: AppUsers
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            
            var appUsers = from u in db.AppUsers select u;
            ViewBag.AppUsersList = appUsers.OrderBy(u => u.FullName).ToList();
            
            return View(appUsers.OrderBy(u => u.FullName).ToPagedList(pageNumber, pageSize));
        }

        // GET: AppUsers/List/5
        // Lists application users
        public ActionResult List(int? appUserID, int? page)
        {
            int pageNumber = (page ?? 1);
            ViewBag.AppUserID = appUserID;

            if (appUserID == null)
            {
                var appUsersUnfiltered = db.AppUsers.OrderBy(p => p.FullName).ToPagedList(pageNumber, pageSize);
                return PartialView(appUsersUnfiltered);
            }

            var appUsers = db.AppUsers.Where(u => u.AppUserID == appUserID).OrderBy(p => p.FullName).ToPagedList(pageNumber, pageSize);

            return PartialView(appUsers);
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
            if (appUser.AspNetUserId != User.Identity.GetUserId() && !User.IsInRole(General.Constants.PROFILE_ADMIN))
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
