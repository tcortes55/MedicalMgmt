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
    public class PrescriptedExamsController : Controller
    {
        private MedicalMgmtDbContext db = new MedicalMgmtDbContext();

        // GET: PrescriptedExams
        public ActionResult Index()
        {
            var prescriptedExams = db.PrescriptedExams.Include(p => p.Exam).Include(p => p.Physician);
            return View(db.PrescriptedExams.ToList());
        }

        // GET: PrescriptedExams/ListExamsByAppointment/5
        public ActionResult ListExamsByAppointment(int? appointmentID)
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

            ViewBag.AppointmentStatusID = appointment.StatusID;
            var examsByAppointment = db.PrescriptedExams
                                           .Where(x => x.AppointmentID == appointmentID)
                                           .Include(p => p.Exam)
                                           .ToList();
            return PartialView(examsByAppointment);
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

        // POST: PrescriptedExams/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Add(int? appointmentID, int? examID, string examDetails, bool examSend)
        {
            if (appointmentID == null || examID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            Appointment appointment = db.Appointments.Find(appointmentID);
            Exam exam = db.Exams.Find(examID);
            if (appointment == null || exam == null)
            {
                return HttpNotFound();
            }
            PrescriptedExam prescriptedExam = new PrescriptedExam();
            prescriptedExam.AppointmentID = appointment.AppointmentID;
            prescriptedExam.ExamID = exam.ExamID;
            prescriptedExam.PatientID = appointment.PatientID;
            prescriptedExam.PhysicianID = appointment.PhysicianID;
            prescriptedExam.Details = examDetails;
            prescriptedExam.SendToPacient = examSend;

            db.PrescriptedExams.Add(prescriptedExam);
            db.SaveChanges();

            //return View();
            return Json(prescriptedExam.PrescriptedExamID, JsonRequestBehavior.AllowGet);
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

            var physicians = new SelectList(db.Physicians, "PhysicianID", "PhysicianID");
            var usPhysicians = new SelectList(db.AppUsers, "AppUserID", "FullName").Where(u => physicians.Any(p => p.Value == u.Value));

            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "Name", prescriptedExam.ExamID);
            ViewBag.PhysicianID = usPhysicians;
            return View(prescriptedExam);
        }

        //// POST: PrescriptedExams/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PrescriptedExamID,AppointmentID,PhysicianID,PatientID,ExamID,Details,ResultDate,Result,SendToPacient,StatusID")] PrescriptedExam prescriptedExam)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(prescriptedExam).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(prescriptedExam);
        //}

        // POST: PrescriptedMedicines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PrescriptedMedicineID,AppointmentID,PhysicianID,PatientID,MedicineID,Posology")] PrescriptedMedicine prescriptedMedicine)
        public ActionResult Edit(int? id, string examDetails, bool examSend)
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

            prescriptedExam.Details = examDetails;
            prescriptedExam.SendToPacient = examSend;

            db.Entry(prescriptedExam).State = EntityState.Modified;
            db.SaveChanges();

            //return RedirectToAction("Index");
            return Json(prescriptedExam.PrescriptedExamID, JsonRequestBehavior.AllowGet);
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
        //[ValidateAntiForgeryToken]
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
