using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;

namespace ICT3104_Group4_SMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private GradesGateway ggw = new GradesGateway();
      

        //View Grade
        public ActionResult ViewGrade()
        {
            var userID = User.Identity.GetUserId();
            ViewBag.ModulesGrades = ggw.SelectGrades(userID);


            return View();
        }
    }
}