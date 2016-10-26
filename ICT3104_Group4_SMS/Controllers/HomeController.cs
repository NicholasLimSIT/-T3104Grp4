using Microsoft.AspNet.Identity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using ICT3104_Group4_SMS.Models;

namespace ICT3104_Group4_SMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        public ActionResult Index()
        {

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("ViewGrade", "Student");
            }
            if (User.IsInRole("HOD"))
            {
                return RedirectToAction("RecommendationsView", "HOD");
            }
            if (User.IsInRole("Lecturer"))
            {
                return RedirectToAction("ModuleIndex", "Lecturer");
            }

            else { 
            return View();
        }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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