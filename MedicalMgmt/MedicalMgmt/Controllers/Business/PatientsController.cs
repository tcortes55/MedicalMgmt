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
    public class PatientsController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        //// GET: Patients
        //public ActionResult Index()
        //{
        //    return View(db.Patients.ToList());
        //}

        // GET: Patients
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FullNameSortParam = String.IsNullOrEmpty(sortOrder) ? "FullName_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var patients = from p in db.Patients select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.FullName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "FullName_desc":
                    patients = patients.OrderByDescending(p => p.FullName);
                    break;
                default:
                    patients = patients.OrderBy(p => p.FullName);
                    break;
            }

            var pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
            int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);
            int pageNumber = (page ?? 1);
            
            return View(patients.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create([Bind(Include = "PatientID,FullName,BirthDate,Telephone,Email,Rg,Cpf,Address,Allergies,FamilyMedicalHistory,LongTermMedication,RegisterDate")] Patient patient)
        {
            patient.RegisterDate = DateTime.Now;
            patient.Active = true;
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
