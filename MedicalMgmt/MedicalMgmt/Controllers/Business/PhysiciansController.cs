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
using MedicalMgmt.ViewModels;
using MedicalMgmt.General;

namespace MedicalMgmt.Controllers.Business
{
    [Authorize]
    public class PhysiciansController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        private static string pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
        int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);

        //// GET: Physicians
        //public ActionResult Index()
        //{
        //    var physicians = db.Physicians.Include(p => p.User);
        //    return View(physicians.ToList());
        //}

        // GET: Physicians
        public ActionResult Index(int? page)
        {
            var physicians = db.Physicians.Where(p => p.AppUser.Active).Include(p => p.AppUser);
            ViewBag.PhysicianList = physicians.OrderBy(p => p.AppUser.FullName).ToList();

            int pageNumber = (page ?? 1);

            return View(physicians.OrderBy(p => p.AppUser.FullName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Physicians
        public ActionResult CreateAppointment(int? patientID, int? physicianID, int? page)
        {
            ViewBag.PhysicianList = db.Physicians.Where(p => p.AppUser.Active).Include(a => a.AppUser).OrderBy(p => p.AppUser.FullName).ToList();
            
            int pageNumber = (page ?? 1);

            var viewModel = new SelectPhysicianData();
            viewModel.Patient = db.Patients.Find(patientID);
            viewModel.Physicians = db.Physicians.Where(p => p.AppUser.Active)
                                                .Include(p => p.Appointment)
                                                .OrderBy(p => p.AppUser.FullName)
                                                .ToPagedList(pageNumber, pageSize);


            //viewModel.Physicians = viewModel.Physicians.OrderBy(p => p.AppUser.FullName).ToPagedList(pageNumber, pageSize);

            viewModel.Appointment = new Appointment();
            viewModel.Appointment.PatientID = patientID.Value;

            ViewBag.PatientID = patientID.Value;
            if (physicianID != null)
            {
                ViewBag.PhysicianID = physicianID.Value;
                viewModel.Appointment.PhysicianID = physicianID.Value;

                var dayBegin = DateTime.Now.Date;
                var dayEnd = dayBegin.AddDays(1);
                var appointments = db.Appointments.Where(a => a.PhysicianID == physicianID
                                                       && a.PlannedStartDate > dayBegin
                                                       && a.PlannedStartDate < dayEnd
                                                       && a.StatusID != Constants.SS_AP_CANCELED);

                var fullSchedule = ConfigurationManager.AppSettings["FullSchedule"].Split(',');
                var existingTimes = new string[] { }.ToList();
                foreach (Appointment ap in appointments)
                {
                    existingTimes.Add(ap.PlannedStartDate.Hour.ToString().PadLeft(2, '0')
                                      + ":"
                                      + ap.PlannedStartDate.Minute.ToString().PadLeft(2, '0')
                                      );
                }

                var availableTimesForDay = fullSchedule.Except(existingTimes).ToList();

                ViewBag.AvailableToday = availableTimesForDay;
            }

            return View(viewModel);
        }

        // GET
        public ActionResult FilteredPhysicians(int? patientID, int? physicianID, string expertise, int? page)
        {
            if (patientID != null)
            {
                ViewBag.PatientID = patientID.Value;
            }
            int pageNumber = (page ?? 1);

            var viewModel = new SelectPhysicianData();
            viewModel.Physicians = db.Physicians.Where(p => p.AppUser.Active)
                                                .Include(p => p.Appointment)
                                                .OrderBy(p => p.AppUser.FullName)
                                                .ToPagedList(pageNumber, pageSize);

            if (physicianID != null)
            {
                viewModel.Physicians = viewModel.Physicians
                                                .Where(p => p.PhysicianID == physicianID)
                                                .OrderBy(p => p.AppUser.FullName)
                                                .ToPagedList(pageNumber, pageSize);
            }

            if (!String.IsNullOrEmpty(expertise))
            {
                viewModel.Physicians = viewModel.Physicians
                                                .Where(p => p.Expertise.ToLower() == expertise.ToLower())
                                                .OrderBy(p => p.AppUser.FullName)
                                                .ToPagedList(pageNumber, pageSize);
            }

            //viewModel.Physicians = viewModel.Physicians.ToList();
            
            return PartialView(viewModel);
        }
        
        // GET: Physicians
        public ActionResult GetAppointmentsByDay(int? patientID, int? physicianID, int? page)
        {
            ViewBag.PhysicianList = db.Physicians.Include(a => a.AppUser).OrderBy(a => a.AppUser.FullName).ToList();
            int pageNumber = (page ?? 1);

            var viewModel = new SelectPhysicianData();
            viewModel.Patient = db.Patients.Find(patientID);
            viewModel.Physicians = db.Physicians.Where(p => p.AppUser.Active)
                                                .Include(p => p.Appointment)
                                                .OrderBy(p => p.AppUser.FullName)
                                                .ToPagedList(pageNumber, pageSize);

            //viewModel.Physicians = viewModel.Physicians.ToList();
            viewModel.Appointment = new Appointment();
            viewModel.Appointment.PatientID = patientID.Value;

            ViewBag.PatientID = patientID.Value;
            if (physicianID != null)
            {
                ViewBag.PhysicianID = physicianID.Value;
                viewModel.Appointment.PhysicianID = physicianID.Value;

                var dayBegin = DateTime.Now.Date;
                var dayEnd = dayBegin.AddDays(1);
                var appointments = db.Appointments.Where(a => a.PhysicianID == physicianID
                                                       && a.PlannedStartDate > dayBegin
                                                       && a.PlannedStartDate < dayEnd
                                                       && a.StatusID != Constants.SS_AP_CANCELED);

                var fullSchedule = ConfigurationManager.AppSettings["FullSchedule"].Split(',');
                var existingTimes = new string[] { }.ToList();
                foreach (Appointment ap in appointments)
                {
                    existingTimes.Add(ap.PlannedStartDate.Hour.ToString().PadLeft(2, '0')
                                      + ":"
                                      + ap.PlannedStartDate.Minute.ToString().PadLeft(2, '0')
                                      );
                }

                var availableTimesForDay = fullSchedule.Except(existingTimes).ToList();

                ViewBag.AvailableToday = availableTimesForDay;
            }

            return View(viewModel);
        }

        // GET: Physicians/DetailsRedirect/5
        public ActionResult DetailsRedirect(string userId)
        {
            int appUserID = db.AppUsers.Where(u => u.AspNetUserId == userId).Select(u => u.AppUserID).SingleOrDefault();

            return RedirectToAction("Details", new { id = appUserID });
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
            SelectList users = new SelectList(db.AppUsers, "AppUserID", "Username");
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

            ViewBag.PhysicianID = new SelectList(db.AppUsers, "AppUserID", "Username", physician.PhysicianID);
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
            ViewBag.PhysicianID = new SelectList(db.AppUsers, "AppUserID", "Username", physician.PhysicianID);
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
                //return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = physician.PhysicianID });
            }
            ViewBag.PhysicianID = new SelectList(db.AppUsers, "AppUserID", "Username", physician.PhysicianID);
            return View(physician);
        }

        // GET: Physicians/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Physician physician = db.Physicians.Find(id);
        //    if (physician == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(physician);
        //}

        //// POST: Physicians/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Physician physician = db.Physicians.Find(id);
        //    db.Physicians.Remove(physician);
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
