﻿using System;
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
    public class PhysiciansController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        //// GET: Physicians
        //public ActionResult Index()
        //{
        //    var physicians = db.Physicians.Include(p => p.User);
        //    return View(physicians.ToList());
        //}

        // GET: Physicians
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FullnameSortParam = String.IsNullOrEmpty(sortOrder) ? "Fullname_desc" : "";
            ViewBag.ExpertiseSortParam = sortOrder == "Expertise" ? "Expertise_desc" : "Expertise";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var physicians = db.Physicians.Include(p => p.AppUser);

            if (!String.IsNullOrEmpty(searchString))
            {
                physicians = physicians.Where(p => p.AppUser.FullName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Fullname_desc":
                    physicians = physicians.OrderByDescending(p => p.AppUser.FullName);
                    break;
                case "Expertise":
                    physicians = physicians.OrderBy(p => p.Expertise);
                    break;
                case "Expertise_desc":
                    physicians = physicians.OrderByDescending(p => p.Expertise);
                    break;
                default:
                    physicians = physicians.OrderBy(p => p.AppUser.FullName);
                    break;
            }

            var pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
            int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);
            int pageNumber = (page ?? 1);

            return View(physicians.ToPagedList(pageNumber, pageSize));
            //var physicians = db.Physicians.Include(p => p.User);
            //return View(physicians.ToList());
        }

        // GET: Physicians
        public ActionResult CreateAppointment(/*string sortOrder, string currentFilter, string searchString, int? page,*/ int? patientID, int? physicianID)
        {
            /* ViewBag.CurrentSort = sortOrder;
             ViewBag.UsernameSortParam = String.IsNullOrEmpty(sortOrder) ? "Username_desc" : "";
             ViewBag.ExpertiseSortParam = sortOrder == "Expertise" ? "Expertise_desc" : "Expertise";

             if (searchString != null)
             {
                 page = 1;
             }
             else
             {
                 searchString = currentFilter;
             }

             ViewBag.CurrentFilter = searchString;

             var physicians = db.Physicians.Include(p => p.User);

             if (!String.IsNullOrEmpty(searchString))
             {
                 physicians = physicians.Where(p => p.User.Username.Contains(searchString));
             }

             switch (sortOrder)
             {
                 case "Username_desc":
                     physicians = physicians.OrderByDescending(p => p.User.Username);
                     break;
                 case "Expertise":
                     physicians = physicians.OrderBy(p => p.Expertise);
                     break;
                 case "Expertise_desc":
                     physicians = physicians.OrderByDescending(p => p.Expertise);
                     break;
                 default:
                     physicians = physicians.OrderBy(p => p.User.Username);
                     break;
             }

             var pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
             int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);
             int pageNumber = (page ?? 1);

             return View(physicians.ToPagedList(pageNumber, pageSize));*/

            var viewModel = new SelectPhysicianData();
            viewModel.Patient = db.Patients.Find(patientID);
            viewModel.Physicians = db.Physicians.Where(p => p.AppUser.Active)
                                                .Include(p => p.Appointment)
                                                .ToList();
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
