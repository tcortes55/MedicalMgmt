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
using PagedList;
using Microsoft.AspNet.Identity;

namespace MedicalMgmt.Controllers.Business
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        private static string pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
        int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);
        
        public int GetAppUserIdFromAspNetId(string aspNetUserId)
        {
            int appUserId = db.AppUsers.Where(u => u.AspNetUserId == aspNetUserId).Select(u => u.AppUserID).SingleOrDefault();

            return appUserId;
        }

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
            if (appointment.StatusID != General.Constants.SS_AP_CANCELED && appointment.StatusID != General.Constants.SS_AP_FINISHED)
            {
                var nextStatusDescription = db.Statuses.Find(appointment.StatusID + 1).StatusDescription; //TODO: remove magic number
                ViewBag.NextStatusID = appointment.StatusID + 1;
                ViewBag.NextStatusDescription = nextStatusDescription;
            }

            ViewBag.IsNewAppointment = TempData["newAppointment"] ?? 0;
            ViewBag.MedicineList = db.Medicines.Where(m => m.Active == true).OrderBy(m => m.CommercialName).ToList();
            ViewBag.ExamList = db.Exams.OrderBy(e => e.Name).ToList();

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
                appointment.StatusID = General.Constants.SS_AP_CANCELED;
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
            if (appointment.StatusID != General.Constants.SS_AP_CANCELED && appointment.StatusID != General.Constants.SS_AP_FINISHED)
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
                    case General.Constants.SS_AP_PLANNED:
                        appointment.PatientArrivingDate = DateTime.Now;
                        break;
                    case General.Constants.SS_AP_PATIENT_WAITING:
                        appointment.ActualStartDate = DateTime.Now;
                        break;
                    case General.Constants.SS_AP_ONGOING:
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
                                                       && a.StatusID != General.Constants.SS_AP_CANCELED);

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
            
            return PartialView(appointments.OrderBy(x => x.PlannedStartDate).ToList());
        }

        // GET: Appointments
        public ActionResult ListAll()
        {
            //var appointments = db.Appointments.Include(a => a.Patient).Include(a => a.Physician).Include(a => a.AppUser);
            //return View(appointments.ToList());
            ViewBag.PhysicianList = db.Physicians.Include(a => a.AppUser).OrderBy(p => p.AppUser.FullName).ToList();
            ViewBag.PatientList = db.Patients.OrderBy(p => p.FullName).ToList();
            return View();
        }

        // GET: Appointments/ListByDate/5
        // Lists appointments from the specified interval
        public ActionResult ListByDate(int? patientID, int? physicianID, string startDate, string endDate, int? page)
        {
            var lStartDate = DateTime.ParseExact(startDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var lEndDate = DateTime.ParseExact(endDate, "yyyy/MM/dd", CultureInfo.InvariantCulture).AddDays(1);
            int pageNumber = (page ?? 1);

            if (physicianID == null && patientID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (lStartDate == null || lEndDate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if ((lEndDate - lStartDate).TotalDays > 365)
            {
                //TODO: display message informing user that it's not possible to query for more than 365 days
            }

            ViewBag.PatientID = patientID;
            ViewBag.PhysicianID = physicianID;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            
            var appointments = db.Appointments.Where(a => (a.PlannedStartDate > lStartDate)
                                                       && (a.PlannedStartDate < lEndDate));

            if (patientID != null)
            {
                appointments = appointments.Where(a => a.PatientID == patientID);
            }
            if (physicianID != null)
            {
                appointments = appointments.Where(a => a.PhysicianID == physicianID);
            }

            var appointmentsPaged = appointments.OrderBy(x => x.PlannedStartDate).ToPagedList(pageNumber, pageSize);

            return PartialView(appointmentsPaged);
        }

        // GET: Appointments/ListPrevious/5
        // Lists appointments for the previous 30 days
        public ActionResult ListPrevious(int? patientID, int? physicianID, int? page)
        {
            if (physicianID == null && patientID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.PatientID = patientID;
            ViewBag.PhysicianID = physicianID;
            int pageNumber = (page ?? 1);

            var begin = DateTime.Today.AddDays(-30);
            var end = DateTime.Today.AddDays(31); //in case a patient arrives before his/her appointment's date, it'll still be considered

            var appointments = db.Appointments.Where(a => (a.PatientID == patientID || patientID == null)
                                                       && (a.PhysicianID == physicianID || physicianID == null)
                                                       && (a.PlannedStartDate > begin)
                                                       && (a.PlannedStartDate < end)
                                                       && (a.StatusID == General.Constants.SS_AP_FINISHED))
                                              .OrderByDescending(x => x.PlannedStartDate)
                                              .ToPagedList(pageNumber, pageSize);

            return PartialView(appointments);
        }

        // GET: Appointments/ListFuture/5
        // Lists future appointments (appointments are only allowed to be scheduled up to 30 days in advance)
        public ActionResult ListFuture(int? patientID, int? physicianID, int? page)
        {
            if (physicianID == null && patientID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.PatientID = patientID;
            ViewBag.PhysicianID = physicianID;
            int pageNumber = (page ?? 1);

            var begin = DateTime.Today;
            var end = begin.AddDays(31);

            var appointments = db.Appointments.Where(a => (a.PatientID == patientID || patientID == null)
                                                       && (a.PhysicianID == physicianID || physicianID == null)
                                                       && (a.PlannedStartDate > begin)
                                                       && (a.PlannedStartDate < end)
                                                       && (a.StatusID == General.Constants.SS_AP_PLANNED))
                                              .OrderBy(a => a.PlannedStartDate)
                                              .ToPagedList(pageNumber, pageSize);

            return PartialView(appointments);
        }

        // GET: Appointments/ListFuture/5
        // Lists future appointments (appointments are only allowed to be scheduled up to 30 days in advance)
        public ActionResult ListWaiting(int? patientID, int? physicianID, int? page)
        {
            if (physicianID == null && patientID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.PatientID = patientID;
            ViewBag.PhysicianID = physicianID;
            int pageNumber = (page ?? 1);

            var appointments = db.Appointments.Where(a => (a.PatientID == patientID || patientID == null)
                                                       && (a.PhysicianID == physicianID || physicianID == null)
                                                       && (a.PlannedStartDate > DateTime.Today)
                                                       && (a.StatusID == General.Constants.SS_AP_PATIENT_WAITING ||
                                                           a.StatusID == General.Constants.SS_AP_ONGOING))
                                              .OrderBy(x => x.PlannedStartDate)
                                              .OrderByDescending(x => x.StatusID)
                                              .ToPagedList(pageNumber, pageSize);
            
            return PartialView(appointments);
        }

        //// GET: Appointments/Create
        //public ActionResult Create()
        //{
        //    ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName");
        //    ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise");
        //    ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "Username");
        //    return View();
        //}

        //// POST: Appointments/Create2
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create2([Bind(Include = "AppointmentID,PhysicianID,PatientID,AppUserID,RegistrationDate,PlannedStartDate,PlannedEndDate,Anamnesis,PatientArrivingDate,ActualStartDate,ActualEndDate,StatusID")] Appointment appointment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Appointments.Add(appointment);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", appointment.PatientID);
        //    ViewBag.PhysicianID = new SelectList(db.Physicians, "PhysicianID", "Expertise", appointment.PhysicianID);
        //    ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "Username", appointment.AppUserID);
        //    return View(appointment);
        //}

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentID,PhysicianID,PatientID,PlannedStartDate")] Appointment appointment)
        {
            appointment.AppUserID = GetAppUserIdFromAspNetId(User.Identity.GetUserId());
            appointment.RegistrationDate = DateTime.Now;
            appointment.PlannedEndDate = appointment.PlannedStartDate.AddMinutes(20); //TODO: remove magic number
            appointment.StatusID = General.Constants.SS_AP_PLANNED;

            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                TempData["newAppointment"] = 1;
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

        //// GET: Appointments/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Appointment appointment = db.Appointments.Find(id);
        //    if (appointment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(appointment);
        //}

        //// POST: Appointments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Appointment appointment = db.Appointments.Find(id);
        //    db.Appointments.Remove(appointment);
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
