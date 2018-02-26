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
    public class PrescriptedMedicinesController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        // GET: PrescriptedMedicines
        public ActionResult Index()
        {
            var prescriptedMedicines = db.PrescriptedMedicines.Include(p => p.Medicine).Include(p => p.Physician);
            return View(prescriptedMedicines.ToList());
        }

        // GET: PrescriptedMedicines/ListMedicinesByAppointment/5
        public ActionResult ListMedicinesByAppointment(int? appointmentID)
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

            ViewBag.MedicineList = db.Medicines.ToList();
            var medicinesByAppointment = db.PrescriptedMedicines
                                           .Where(x => x.AppointmentID == appointmentID)
                                           .Include(p => p.Medicine)
                                           .ToList();
            return PartialView(medicinesByAppointment);
        }

        // GET: PrescriptedMedicines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrescriptedMedicine prescriptedMedicine = db.PrescriptedMedicines.Find(id);
            if (prescriptedMedicine == null)
            {
                return HttpNotFound();
            }
            return View(prescriptedMedicine);
        }

        // GET: PrescriptedMedicines/Create
        public ActionResult Create()
        {
            var physicians = new SelectList(db.Physicians, "PhysicianID", "PhysicianID");
            var usPhysicians = new SelectList(db.AppUsers, "AppUserID", "FullName").Where(u => physicians.Any(p => p.Value == u.Value));
            
            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "CommercialName");
            ViewBag.PhysicianID = usPhysicians;
            return View();
        }

        // POST: PrescriptedMedicines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptedMedicineID,AppointmentID,PhysicianID,PatientID,MedicineID,Posology")] PrescriptedMedicine prescriptedMedicine)
        {
            if (ModelState.IsValid)
            {
                db.PrescriptedMedicines.Add(prescriptedMedicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var physicians = new SelectList(db.Physicians, "PhysicianID", "PhysicianID");
            var usPhysicians = new SelectList(db.AppUsers, "AppUserID", "FullName").Where(u => physicians.Any(p => p.Value == u.Value));

            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "CommercialName", prescriptedMedicine.MedicineID);
            ViewBag.PhysicianID = usPhysicians;
            return View(prescriptedMedicine);
        }

        // POST: PrescriptedMedicines/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Add(int? appointmentID, int? medicineID, string posology)
        {
            if (appointmentID == null || medicineID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            Appointment appointment = db.Appointments.Find(appointmentID);
            Medicine medicine = db.Medicines.Find(medicineID);
            if (appointment == null || medicine == null)
            {
                return HttpNotFound();
            }
            PrescriptedMedicine prescriptedMedicine = new PrescriptedMedicine();
            prescriptedMedicine.AppointmentID = appointment.AppointmentID;
            prescriptedMedicine.MedicineID = medicine.MedicineID;
            prescriptedMedicine.PatientID = appointment.PatientID;
            prescriptedMedicine.PhysicianID = appointment.PhysicianID;
            prescriptedMedicine.Posology = posology;

            db.PrescriptedMedicines.Add(prescriptedMedicine);
            db.SaveChanges();

            return View();
        }

        // GET: PrescriptedMedicines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrescriptedMedicine prescriptedMedicine = db.PrescriptedMedicines.Find(id);
            if (prescriptedMedicine == null)
            {
                return HttpNotFound();
            }

            var physicians = new SelectList(db.Physicians, "PhysicianID", "PhysicianID");
            var usPhysicians = new SelectList(db.AppUsers, "AppUserID", "FullName").Where(u => physicians.Any(p => p.Value == u.Value));

            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "CommercialName", prescriptedMedicine.MedicineID);
            ViewBag.PhysicianID = usPhysicians;
            return View(prescriptedMedicine);
        }

        // POST: PrescriptedMedicines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptedMedicineID,AppointmentID,PhysicianID,PatientID,MedicineID,Posology")] PrescriptedMedicine prescriptedMedicine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescriptedMedicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            var physicians = new SelectList(db.Physicians, "PhysicianID", "PhysicianID");
            var usPhysicians = new SelectList(db.AppUsers, "AppUserID", "FullName").Where(u => physicians.Any(p => p.Value == u.Value));

            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "CommercialName", prescriptedMedicine.MedicineID);
            ViewBag.PhysicianID = usPhysicians;
            return View(prescriptedMedicine);
        }

        // GET: PrescriptedMedicines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrescriptedMedicine prescriptedMedicine = db.PrescriptedMedicines.Find(id);
            if (prescriptedMedicine == null)
            {
                return HttpNotFound();
            }
            return View(prescriptedMedicine);
        }

        // POST: PrescriptedMedicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrescriptedMedicine prescriptedMedicine = db.PrescriptedMedicines.Find(id);
            db.PrescriptedMedicines.Remove(prescriptedMedicine);
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
