using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ICT3104_Group4_SMS.DAL;
using System.Net;
using Microsoft.Owin.Security;

namespace ICT3104_Group4_SMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        private SmsContext db = new SmsContext();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Index()
        {
            
            var currentUserId = User.Identity.GetUserId();
            Models.ApplicationUser Applicationuser = db.Users.Find(currentUserId);
            //check if status is lock
            if (Applicationuser != null && Applicationuser.lockStatus.Equals("Lock") || Applicationuser != null && Applicationuser.status.Equals("inactive"))
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session.Clear();
                return RedirectToAction("AccountLockout", "Home");

            }

            if (User.IsInRole("Student"))
            {
                return RedirectToAction("ViewGrade", "Student");
            }
            else
            {
                if (Session["Verified"] == null)
                    return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = Request.Url.AbsolutePath });

                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("HOD"))
                {
                    return RedirectToAction("RecommendationsView", "HOD");
                }
                else if (User.IsInRole("Lecturer"))
                {
                    return RedirectToAction("ModuleIndex", "Lecturer");
                }
            }

            return RedirectToAction("Login","Account");
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
        public ActionResult AccountLockout()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}