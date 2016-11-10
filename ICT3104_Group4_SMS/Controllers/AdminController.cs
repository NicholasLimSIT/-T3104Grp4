using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Collections;


namespace ICT3104_Group4_SMS.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private SmsContext db = new SmsContext();
        private ApplicationUserManager _userManager;

        internal IDataGateway<ApplicationUser> ApplicationUserGateway;
        internal IDataGateway<ArchivedRecord> ArchiveGateway;

        // check if user has passed 2FA authentication. true = did not pass. false = passed.
        public bool IfUserSkipTwoFA()
        {
            if (Session == null)
                return true;
            else if (Session["Verified"] == null)
                return true;

            return false;
        }

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

        public int T { get; private set; }

        // GET: Admin
        public ActionResult Index()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            ViewBag.List = ((ApplicationUserDataGateway)ApplicationUserGateway).searchUsers();

            ((ApplicationUserDataGateway)ApplicationUserGateway).lockExpireUsers();
            return View();
  
        }

        // GET: /Admin/ArchivedStudentsView
        public ActionResult ArchivedStudentsView()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            return View(db.ArchivedRecords.ToList());
        }

        public ActionResult Archive()
        {
            int result;
          
            result = ((ArchiveDataGateway)ArchiveGateway).ArchivedRecord();
            
            if (result == 0)
            {
                TempData["Message"] = "activeFail";
            }
            else
            {
                TempData["Message"] = "activeSuccess";
            }
            return RedirectToAction("ArchivedStudentsView"); 
        }


        public ActionResult BackupArchived()
        {

            GridView gv = new GridView();
            gv.DataSource = db.ArchivedRecords.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ArchivedRecords.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("AdminBackup");

        }
        public ActionResult BackupUser()
        {
            GridView gv = new GridView();
            gv.DataSource = db.Users.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Users.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("AdminBackup");
        }
        public ActionResult BackupModule()
        {
            GridView gv = new GridView();
            gv.DataSource = db.Modules.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Module.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("AdminBackup");
        }
        public ActionResult BackupLecturerModule()
        {
            GridView gv = new GridView();
            gv.DataSource = db.Lecturer_Module.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Lecturer_Module.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("AdminBackup");
        }
        public ActionResult BackupGrades()
        {
            GridView gv = new GridView();
            gv.DataSource = db.Grades.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Grades.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("AdminBackup");
        }
        public ActionResult BackupRecommendations()
        {
            GridView gv = new GridView();
            gv.DataSource = db.Recommendations.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Recommendations.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("AdminBackup");
        }

       

        // GET: /Admin/CreateAccount
        public ActionResult CreateAccount()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

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
                var user = new ApplicationUser { UserName = model.Email.Substring(0, index), Email = model.Email,year = DateTime.Now.Year.ToString(), lastChangedPWd = DateTime.Now };
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

        // GET: /Admin/SearchAccountParticulars
        [HttpGet]
        public ActionResult SearchAccountParticulars(string name)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

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

        // GET: /Admin/SearchLockAccount
        [HttpGet]
        public ActionResult SearchLockAccount(string name)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (name != null)
            {
                ViewBag.LockUserList = ((ApplicationUserDataGateway)ApplicationUserGateway).searchLockUser(name);
            }
            else
            {
                ViewBag.LockUserList = ((ApplicationUserDataGateway)ApplicationUserGateway).ListLockUsers();
            }
            return View();
        }
        public ActionResult UnlockAccount(string id)
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
            if (Applicationuser != null)
            {
                Applicationuser.lockStatus = "Unlock";
                db.Entry(Applicationuser).State = EntityState.Modified;
                db.SaveChanges();
            }
           
            return RedirectToAction("SearchLockAccount");
        }



        // GET: /Admin/EditAccount
        public ActionResult EditAccount(string id)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

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
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

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
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

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