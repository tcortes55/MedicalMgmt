using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalMgmt.Models;
using MedicalMgmt.General;

namespace MedicalMgmt.Controllers.Business
{
    public class AppointmentsController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        // GET: Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Patient).Include(a => a.Physician).Include(a => a.User);
            return View(appointments.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Details/5
        public ActionResult GetByPhysicianIdAndDate(int? physicianID, DateTime? date)
        {
            date = DateTime.Now.Date;
            var date2 = DateTime.Now.AddDays(1).Date;
            if (physicianID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appointments = db.Appointments.Where(a => a.PhysicianID == physicianID
                                                       && a.PlannedStartDate > date
                                                       && a.PlannedStartDate < date2
                                                       && a.StatusID != Constants.SS_AP_CANCELED);

            var lala = new DateTime[appointments.Count()].ToList();
            foreach (Appointment ap in appointments)
            {
                lala.Add(ap.PlannedStartDate);
            }

            return Json(lala, JsonRequestBehavior.AllowGet);
            //if (appointment == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName");
            ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Appointments/Create2
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "AppointmentID,PhysicianID,PatientID,UserID,RegistrationDate,PlannedStartDate,PlannedEndDate,Anamnesis,PatientArrivingDate,ActualStartDate,ActualEndDate,StatusID")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", appointment.PatientID);
            ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise", appointment.PhysicianID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentID,PhysicianID,PatientID,PlannedStartDate")] Appointment appointment)
        {
            appointment.UserID = 1;
            appointment.RegistrationDate = DateTime.Now;
            appointment.PlannedEndDate = appointment.PlannedStartDate.AddMinutes(20);
            appointment.StatusID = Constants.SS_AP_PLANNED;

            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = appointment.AppointmentID } );
            }

            //ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", appointment.PatientID);
            //ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise", appointment.PhysicianID);
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", appointment.PatientID);
            ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise", appointment.PhysicianID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentID,PhysicianID,PatientID,UserID,RegistrationDate,PlannedStartDate,PlannedEndDate,Anamnesis,PatientArrivingDate,ActualStartDate,ActualEndDate,StatusID")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", appointment.PatientID);
            ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise", appointment.PhysicianID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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
