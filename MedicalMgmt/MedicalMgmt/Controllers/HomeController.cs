﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MedicalMgmt.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole(MedicalMgmt.General.Constants.PROFILE_PHYSICIAN))
            {
                return RedirectToAction("DetailsRedirect", "Physicians", new { userId = User.Identity.GetUserId() });
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}