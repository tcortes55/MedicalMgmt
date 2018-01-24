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

            return View(physicians.ToPagedList(pageNumber, pageSize));
            //var physicians = db.Physicians.Include(p => p.User);
            //return View(physicians.ToList());
        }

        // GET: Physicians
        public ActionResult SelectPhysician(/*string sortOrder, string currentFilter, string searchString, int? page, int? patientID,*/ int? physicianID)
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
            var viewModel = new SelectPhysicianData(); //db.Physicians.Include(p => p.User);

            viewModel.Physicians = db.Physicians.Where(p => p.User.Active == true)
                                                .Include(i => i.Appointment)
                                                .ToList();

            if (physicianID != null)
            {
                ViewBag.PhysicianID = physicianID.Value;
                viewModel.Appointments = viewModel.Physicians
                                                  .Where(p => p.PhysicianID == physicianID.Value)
                                                  .Single()
                                                  .Appointment
                                                  .ToList();
                //physicians = physicians.Where(p => p.PhysicianID == physicianID);
            }

            return View(viewModel);
        }

        //public ActionResult SelectDateTime(int? id)
        public ActionResult SelectDateTime()
        {
            //return PartialView(db.Appointments.Where(a => a.PhysicianID == id && a.PlannedStartDate > DateTime.Now));
            return PartialView("_SelectDateTime");
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
            SelectList users = new SelectList(db.Users, "UserID", "Username");
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

            ViewBag.PhysicianID = new SelectList(db.Users, "UserID", "Username", physician.PhysicianID);
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
            ViewBag.PhysicianID = new SelectList(db.Users, "UserID", "Username", physician.PhysicianID);
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
                return RedirectToAction("Index");
            }
            ViewBag.PhysicianID = new SelectList(db.Users, "UserID", "Username", physician.PhysicianID);
            return View(physician);
        }

        // GET: Physicians/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Physicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Physician physician = db.Physicians.Find(id);
            db.Physicians.Remove(physician);
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
