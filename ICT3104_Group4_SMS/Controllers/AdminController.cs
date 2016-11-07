using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace ICT3104_Group4_SMS.Controllers
{

   [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private SmsContext db = new SmsContext();
        private ApplicationUserManager _userManager;

        internal IDataGateway<ApplicationUser> ApplicationUserGateway;
        internal IDataGateway<ArchivedRecord> ArchiveGateway;


        public AdminController()
        {
            ApplicationUserGateway = new ApplicationUserDataGateway();
            ArchiveGateway = new ArchiveDataGateway();
        }

        public AdminController(ApplicationUserManager userManager)
        {
            ApplicationUserGateway = new ApplicationUserDataGateway();
           
            UserManager = userManager;
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

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.List = ((ApplicationUserDataGateway)ApplicationUserGateway).searchUsers();


            return View();
  
        }
        public ActionResult ArchivedStudentsView()
        {

            return View(db.ArchivedRecords.ToList());
        }
        public ActionResult Archive()
        {
            ((ArchiveDataGateway)ArchiveGateway).ArchivedRecord();
            return RedirectToAction("ArchivedStudentsView"); 
        }
        // GET: /Admin/CreateAccount
        public ActionResult CreateAccount()
        {
            return View();
        }


        //POST: /Admin/CreateAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAccount(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.createNotif = "not-active";
                // get the index of @ of the email to remove
                int index = model.Email.IndexOf("@");              
                var user = new ApplicationUser { UserName = model.Email.Substring(0, index), Email = model.Email,year = DateTime.Now.Year.ToString() };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    ViewBag.createNotif = "activeSuccess";
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRole);
                    
                }
                else {
                    ViewBag.createNotif = "activeFail";
                }

            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public ActionResult SearchAccountParticulars(string name)
        {
            if (name != null)
            {
                ViewBag.List = ((ApplicationUserDataGateway)ApplicationUserGateway).searchSpecficUser(name);
            }
            else
            {
                ViewBag.List = ((ApplicationUserDataGateway)ApplicationUserGateway).searchUsers();
            }
            return View();
        }



        // GET: /Admin/EditAccount
        public ActionResult EditAccount(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.ApplicationUser Applicationuser = db.Users.Find(id);
            if (Applicationuser == null)
            {
                return HttpNotFound();
            }
            return View(Applicationuser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount([Bind(Include = "Id,UserName,Email,PhoneNumber")] Models.ApplicationUser Applicationuser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Applicationuser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SearchAccountParticulars");
            }
            return View(Applicationuser);
        }

        // GET: /Admin/DeleteAccount
        public ActionResult DeleteAccount(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.ApplicationUser Applicationuser = db.Users.Find(id);
            if (Applicationuser == null)
            {
                return HttpNotFound();
            }
            return View(Applicationuser);
        }

        [HttpPost, ActionName("DeleteAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            Models.ApplicationUser Applicationuser = db.Users.Find(id);
            

            ((ApplicationUserDataGateway)ApplicationUserGateway).MovedUser(id);


            //db.Users.Remove(Applicationuser);
            //db.SaveChanges();
            return RedirectToAction("SearchAccountParticulars");
        }












        // GET: /Admin/BackupAccount
        public ActionResult AdminBackup()
        {
            return View();
        }


























        //public ActionResult DeleteStudent(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Models.ApplicationUser Applicationuser = db.Users.Find(id);
        //    if (Applicationuser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Applicationuser);
        //}

        //[HttpPost, ActionName("DeleteStudent")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Models.ApplicationUser Applicationuser = db.Users.Find(id);
        //    db.Users.Remove(Applicationuser);
        //    db.SaveChanges();
        //    return RedirectToAction("SearchStudentParticulars");
        //}
    }
}