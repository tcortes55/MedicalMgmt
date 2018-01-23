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

namespace MedicalMgmt.Controllers.Application
{
    public class UsersController : Controller
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
            ViewBag.UsernameSortParam = String.IsNullOrEmpty(sortOrder) ? "FullName_desc" : "";
            ViewBag.FullNameSortParam = sortOrder == "Username" ? "Username_desc" : "Username";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var users = from u in db.Users select u;

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

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
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
        public ActionResult Create([Bind(Include = "UserID,Username,FullName,Telephone,Email,Rg,Cpf,Address")] User user)
        {
            user.RegisterDate = DateTime.Now;
            user.Active = true;
            if (ModelState.IsValid)
            {
                user.Username = user.Username.ToUpper();
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,FullName,Telephone,Email,Rg,Cpf,Address,RegisterDate,Active")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
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
