using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalMgmt.Models;

namespace MedicalMgmt.Controllers.Business
{
    public class PhysiciansController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        // GET: Physicians
        public ActionResult Index()
        {
            var physicians = db.Physicians.Include(p => p.User);
            return View(physicians.ToList());
        }

        // GET: Physicians/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Physician physician = db.Physicians.Find(id);
            if (physician == null)
            {
                return HttpNotFound();
            }
            return View(physician);
        }

        // GET: Physicians/Create
        public ActionResult Create()
        {
            // Only users who aren't already Physicians can be registered as Physicians
            SelectList users = new SelectList(db.Users, "UserID", "Username");
            SelectList physicians = new SelectList(db.Physicians, "PhysicianID", "PhysicianID");

            var list = users.Where(u => physicians.All(p => p.Value != u.Value));

            ViewBag.PhysicianID = list;
            return View();
        }

        // POST: Physicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhysicianID,Expertise,GraduationUni,GraduationYear")] Physician physician)
        {
            if (ModelState.IsValid)
            {
                db.Physicians.Add(physician);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhysicianID = new SelectList(db.Users, "UserID", "Username", physician.PhysicianID);
            return View(physician);
        }

        // GET: Physicians/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Physician physician = db.Physicians.Find(id);
            if (physician == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhysicianID = new SelectList(db.Users, "UserID", "Username", physician.PhysicianID);
            return View(physician);
        }

        // POST: Physicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhysicianID,Expertise,GraduationUni,GraduationYear")] Physician physician)
        {
            if (ModelState.IsValid)
            {
                db.Entry(physician).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhysicianID = new SelectList(db.Users, "UserID", "Username", physician.PhysicianID);
            return View(physician);
        }

        // GET: Physicians/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Physician physician = db.Physicians.Find(id);
            if (physician == null)
            {
                return HttpNotFound();
            }
            return View(physician);
        }

        // POST: Physicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Physician physician = db.Physicians.Find(id);
            db.Physicians.Remove(physician);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
