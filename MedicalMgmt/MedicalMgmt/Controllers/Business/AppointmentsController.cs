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
using System.Globalization;
using System.Configuration;

namespace MedicalMgmt.Controllers.Business
{
    public class AppointmentsController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        // GET: Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Patient).Include(a => a.Physician).Include(a => a.AppUser);
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
            if (appointment.StatusID != Constants.SS_AP_CANCELED && appointment.StatusID != Constants.SS_AP_FINISHED)
            {
                var nextStatusDescription = db.Statuses.Find(appointment.StatusID + 1).StatusDescription; //TODO: remove magic number
                ViewBag.NextStatusID = appointment.StatusID + 1;
                ViewBag.NextStatusDescription = nextStatusDescription;
            }
            return View(appointment);
        }

        // GET: Appointments/Cancel/5
        public ActionResult Cancel(int? id)
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
            return PartialView(appointment);
        }

        // POST: Appointments/Cancel/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmCancel(int? id)
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

            if (ModelState.IsValid)
            {
                appointment.StatusID = Constants.SS_AP_CANCELED;
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = appointment.AppointmentID });
            }

            return View(appointment);
        }

        // GET: Appointments/ChangeStatus/5
        public ActionResult ChangeStatus(int? appointmentID, int? currStatusID)
        {
            if (appointmentID == null || currStatusID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(appointmentID);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            if (appointment.StatusID != Constants.SS_AP_CANCELED && appointment.StatusID != Constants.SS_AP_FINISHED)
            {
                var nextStatusDescription = db.Statuses.Find(appointment.StatusID + 1).StatusDescription; //TODO: remove magic number
                ViewBag.NextStatusDescription = nextStatusDescription;
            }

            return PartialView(appointment);
        }

        // POST: Appointments/ConfirmChangeStatus/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmChangeStatus(int? appointmentID, int? currStatusID)
        {
            if (appointmentID == null || currStatusID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(appointmentID);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                switch (currStatusID)
                {
                    case Constants.SS_AP_PLANNED:
                        appointment.PatientArrivingDate = DateTime.Now;
                        break;
                    case Constants.SS_AP_PATIENT_WAITING:
                        appointment.ActualStartDate = DateTime.Now;
                        break;
                    case Constants.SS_AP_ONGOING:
                        appointment.ActualEndDate = DateTime.Now;
                        break;
                }

                appointment.StatusID = appointment.StatusID + 1; //TODO: remove magic number
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = appointment.AppointmentID });
            }

            return View(appointment);
        }

        // POST: Appointments/UpdatePatientInfo/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdatePatientInfo(
            int? appointmentID, 
            string allergies, 
            string familyMedicalHistory, 
            string longTermMedication, 
            string anamnesis
        )
        {
            if (appointmentID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(appointmentID);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                appointment.Anamnesis = anamnesis;
                appointment.Patient.Allergies = allergies;
                appointment.Patient.FamilyMedicalHistory = familyMedicalHistory;
                appointment.Patient.LongTermMedication = longTermMedication;
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = appointment.AppointmentID });
            }

            return View(appointment);
        }

        // GET: Appointments/GetByPhysicianIdAndDate/5
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
            var fullSchedule = ConfigurationManager.AppSettings["FullSchedule"].Split(',');
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

        public ActionResult List(int? patientID, int? physicianID)
        {
            ViewBag.PatientID = patientID;
            ViewBag.PhysicianID = physicianID;

            var appointments = db.Appointments.Where(a => (a.PatientID == patientID || patientID == null)
                                                       && (a.PhysicianID == physicianID || physicianID == null));
            
            return PartialView(appointments.OrderByDescending(x => x.PlannedStartDate).ToList());
        }

        public ActionResult ListPrevious(int? patientID, int? physicianID)
        {
            ViewBag.PatientID = patientID;
            ViewBag.PhysicianID = physicianID;

            var appointments = db.Appointments.Where(a => (a.PatientID == patientID || patientID == null)
                                                       && (a.PhysicianID == physicianID || physicianID == null)
                                                       && (a.PlannedStartDate < DateTime.Now)
                                                     );

            return PartialView(appointments.OrderByDescending(x => x.PlannedStartDate).ToList());
        }

        public ActionResult ListFuture(int? patientID, int? physicianID)
        {
            ViewBag.PatientID = patientID;
            ViewBag.PhysicianID = physicianID;

            var appointments = db.Appointments.Where(a => (a.PatientID == patientID || patientID == null)
                                                       && (a.PhysicianID == physicianID || physicianID == null)
                                                       && (a.PlannedStartDate > DateTime.Now)
                                                       && (a.StatusID != Constants.SS_AP_CANCELED)
                                                     );

            return PartialView(appointments.OrderBy(x => x.PlannedStartDate).ToList());
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName");
            ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise");
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "Username");
            return View();
        }

        // POST: Appointments/Create2
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "AppointmentID,PhysicianID,PatientID,AppUserID,RegistrationDate,PlannedStartDate,PlannedEndDate,Anamnesis,PatientArrivingDate,ActualStartDate,ActualEndDate,StatusID")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", appointment.PatientID);
            ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise", appointment.PhysicianID);
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "Username", appointment.AppUserID);
            return View(appointment);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentID,PhysicianID,PatientID,PlannedStartDate")] Appointment appointment)
        {
            appointment.AppUserID = 1; //TODO: replace for current user ID
            appointment.RegistrationDate = DateTime.Now;
            appointment.PlannedEndDate = appointment.PlannedStartDate.AddMinutes(20); //TODO: remove magic number
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
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "Username", appointment.AppUserID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentID,PhysicianID,PatientID,AppUserID,RegistrationDate,PlannedStartDate,PlannedEndDate,Anamnesis,PatientArrivingDate,ActualStartDate,ActualEndDate,StatusID")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", appointment.PatientID);
            ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise", appointment.PhysicianID);
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "Username", appointment.AppUserID);
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
