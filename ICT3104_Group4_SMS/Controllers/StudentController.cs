using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICT3104_Group4_SMS.DAL;
using Microsoft.AspNet.Identity;

namespace ICT3104_Group4_SMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private GradesGateway ggw = new GradesGateway();
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        //View Grade
        public ActionResult ViewGrade()
        {
            var userID = User.Identity.GetUserId();
            ViewBag.ModuleGrades = ggw.SelectGrades(userID);
          
            return View();
        }
    }
}