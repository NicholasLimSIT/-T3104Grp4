using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ICT3104_Group4_SMS.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class LecturerController : Controller
    {
        private SmsContext db = new SmsContext();



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditStudentParticulars()
        {
            return View();
        }

        public ActionResult GradeAssign()
        {
            return View();
        }
    }

   
}