﻿using System;
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
    public class PrescriptedExamsController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        // GET: PrescriptedExams
        public ActionResult Index()
        {
            return View(db.PrescriptedExams.ToList());
        }

        // GET: PrescriptedExams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrescriptedExam prescriptedExam = db.PrescriptedExams.Find(id);
            if (prescriptedExam == null)
            {
                return HttpNotFound();
            }
            return View(prescriptedExam);
        }

        // GET: PrescriptedExams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrescriptedExams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptedExamID,AppointmentID,PhysicianID,PatientID,ExamID,Details,ResultDate,Result,SendToPacient,StatusID")] PrescriptedExam prescriptedExam)
        {
            if (ModelState.IsValid)
            {
                db.PrescriptedExams.Add(prescriptedExam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prescriptedExam);
        }

        // GET: PrescriptedExams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrescriptedExam prescriptedExam = db.PrescriptedExams.Find(id);
            if (prescriptedExam == null)
            {
                return HttpNotFound();
            }
            return View(prescriptedExam);
        }

        // POST: PrescriptedExams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptedExamID,AppointmentID,PhysicianID,PatientID,ExamID,Details,ResultDate,Result,SendToPacient,StatusID")] PrescriptedExam prescriptedExam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescriptedExam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prescriptedExam);
        }

        // GET: PrescriptedExams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrescriptedExam prescriptedExam = db.PrescriptedExams.Find(id);
            if (prescriptedExam == null)
            {
                return HttpNotFound();
            }
            return View(prescriptedExam);
        }

        // POST: PrescriptedExams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrescriptedExam prescriptedExam = db.PrescriptedExams.Find(id);
            db.PrescriptedExams.Remove(prescriptedExam);
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