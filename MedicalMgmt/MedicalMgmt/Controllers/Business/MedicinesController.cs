using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalMgmt.Models;
using System.Configuration;
using PagedList;

namespace MedicalMgmt.Controllers.Business
{
    public class MedicinesController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        //// GET: Medicines
        //public ActionResult Index()
        //{
        //    return View(db.Medicines.ToList());
        //}

        // GET: Medicines
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CommercialNameSortParam = String.IsNullOrEmpty(sortOrder) ? "CommercialName_desc" : "";
            ViewBag.GenericNameSortParam = sortOrder == "GenericName" ? "GenericName_desc" : "GenericName";
            ViewBag.ManufacturerSortParam = sortOrder == "Manufacturer" ? "Manufacturer_desc" : "Manufacturer";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var medicines = from m in db.Medicines select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                medicines = medicines.Where(m => m.GenericName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "CommercialName_desc":
                    medicines = medicines.OrderByDescending(m => m.CommercialName);
                    break;
                case "GenericName":
                    medicines = medicines.OrderBy(m => m.GenericName);
                    break;
                case "GenericName_desc":
                    medicines = medicines.OrderByDescending(m => m.GenericName);
                    break;
                case "Manufacturer":
                    medicines = medicines.OrderBy(m => m.Manufacturer);
                    break;
                case "Manufacturer_desc":
                    medicines = medicines.OrderByDescending(m => m.Manufacturer);
                    break;
                default:
                    medicines = medicines.OrderBy(m => m.CommercialName);
                    break;
            }

            var pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
            int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);
            int pageNumber = (page ?? 1);
            return View(medicines.ToPagedList(pageNumber, pageSize));

            //return View(db.Medicines.ToList());
        }

        // GET: Medicines/List
        public ActionResult List(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CommercialNameSortParam = String.IsNullOrEmpty(sortOrder) ? "CommercialName_desc" : "";
            ViewBag.GenericNameSortParam = sortOrder == "GenericName" ? "GenericName_desc" : "GenericName";
            ViewBag.ManufacturerSortParam = sortOrder == "Manufacturer" ? "Manufacturer_desc" : "Manufacturer";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var medicines = from m in db.Medicines select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                medicines = medicines.Where(m => m.GenericName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "CommercialName_desc":
                    medicines = medicines.OrderByDescending(m => m.CommercialName);
                    break;
                case "GenericName":
                    medicines = medicines.OrderBy(m => m.GenericName);
                    break;
                case "GenericName_desc":
                    medicines = medicines.OrderByDescending(m => m.GenericName);
                    break;
                case "Manufacturer":
                    medicines = medicines.OrderBy(m => m.Manufacturer);
                    break;
                case "Manufacturer_desc":
                    medicines = medicines.OrderByDescending(m => m.Manufacturer);
                    break;
                default:
                    medicines = medicines.OrderBy(m => m.CommercialName);
                    break;
            }

            var pageSizeConfig = ConfigurationManager.AppSettings["PageSize"];
            int pageSize = string.IsNullOrEmpty(pageSizeConfig) ? 5 : Convert.ToInt16(pageSizeConfig);
            int pageNumber = (page ?? 1);
            return PartialView(medicines.ToPagedList(pageNumber, pageSize));

            //return View(db.Medicines.ToList());
        }

        //// GET: Medicines/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Medicine medicine = db.Medicines.Find(id);
        //    if (medicine == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(medicine);
        //}

        // GET: Medicines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicineID,CommercialName,GenericName,Manufacturer,Active")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                medicine.Active = true;
                db.Medicines.Add(medicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicine);
        }

        // GET: Medicines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicineID,CommercialName,GenericName,Manufacturer,Active")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicine);
        }

        //// GET: Medicines/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Medicine medicine = db.Medicines.Find(id);
        //    if (medicine == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(medicine);
        //}

        //// POST: Medicines/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Medicine medicine = db.Medicines.Find(id);
        //    db.Medicines.Remove(medicine);
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
