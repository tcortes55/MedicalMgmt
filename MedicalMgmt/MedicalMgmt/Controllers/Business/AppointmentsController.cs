﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalMgmt.Models;
using MedicalMgmt.General;
using System.Globalization;

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
        public ActionResult GetByPhysicianIdAndDate(int? physicianID, string selectedDate)
        {
            var dayBegin = DateTime.ParseExact(selectedDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var dayEnd = dayBegin.AddDays(1);

            if (physicianID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appointments = db.Appointments.Where(a => a.PhysicianID == physicianID
                                                       && a.PlannedStartDate > dayBegin
                                                       && a.PlannedStartDate < dayEnd
                                                       && a.StatusID != Constants.SS_AP_CANCELED);

            //https://stackoverflow.com/questions/20815252/to-remove-common-string-from-two-string-array-in-net
            var fullSchedule =  new string[] {
                                                        "08:00","08:20","08:40",
                                                        "09:00","09:20","09:40",
                                                        "10:00","10:20","10:40",
                                                        "11:00","11:20","11:40",
                                                        "13:00","13:20","13:40",
                                                        "14:00","14:20","14:40",
                                                        "15:00","15:20","15:40",
                                                        "16:00","16:20","16:40",
                                                        "17:00","17:20","17:40"
                                                      };
            var existingTimes = new string[] { }.ToList();
            foreach(Appointment ap in appointments)
            {
                existingTimes.Add(ap.PlannedStartDate.Hour.ToString().PadLeft(2,'0')
                                  + ":"
                                  + ap.PlannedStartDate.Minute.ToString().PadLeft(2, '0')
                                  );
            }

            var availableTimesForDay = fullSchedule.Except(existingTimes);

            return Json(availableTimesForDay, JsonRequestBehavior.AllowGet);
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
