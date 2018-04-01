using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using MedicalMgmt.Models;
using PagedList;

namespace MedicalMgmt.Controllers.Business
{
    [Authorize]
    public class PatientsController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        private static string pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
        int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);

        //// GET: Patients
        //public ActionResult Index()
        //{
        //    return View(db.Patients.ToList());
        //}

        // GET: Patients
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);

            var patients = from p in db.Patients select p;
            ViewBag.PatientList = patients.OrderBy(p => p.FullName).ToList();

            return View(patients.OrderBy(p => p.FullName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Patients/List/5
        // Lists patients
        public ActionResult List(int? patientID, int? page)
        {
            int pageNumber = (page ?? 1);
            ViewBag.PatientID = patientID;

            if (patientID == null)
            {
                var patientsUnfiltered = db.Patients.OrderBy(p => p.FullName).ToPagedList(pageNumber, pageSize);
                return PartialView(patientsUnfiltered);
            }

            var patients = db.Patients.Where(p => p.PatientID == patientID).OrderBy(p => p.FullName).ToPagedList(pageNumber, pageSize);
            
            return PartialView(patients);
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,FullName,BirthDate,Telephone,Email,Rg,Cpf,Address")] Patient patient)
        {
            patient.RegisterDate = DateTime.Now;
            patient.Active = true;
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = patient.PatientID });
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,FullName,BirthDate,Telephone,Email,Rg,Cpf,Address,Allergies,FamilyMedicalHistory,LongTermMedication,RegisterDate,Active")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = patient.PatientID });
            }
            return View(patient);
        }

        //// GET: Patients/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Patient patient = db.Patients.Find(id);
        //    if (patient == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(patient);
        //}

        //// POST: Patients/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Patient patient = db.Patients.Find(id);
        //    db.Patients.Remove(patient);
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
