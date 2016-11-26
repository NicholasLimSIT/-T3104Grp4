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
using System.Net.Mail;
using SendGrid;
using Microsoft.AspNet.Identity;

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
            //lock those accounts that are over 100 days
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
                var user = new ApplicationUser { FullName = model.FullName, UserName = model.Email.Substring(0, index), Email = model.Email, PhoneNumber = model.PhoneNumber, year = DateTime.Now.Year.ToString(), lastChangedPWd = DateTime.Now, lockStatus = "clear", status ="active" };
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
                Applicationuser.lastChangedPWd = DateTime.Now;
                db.Entry(Applicationuser).State = EntityState.Modified;
                db.SaveChanges();
            }

            TempData["SearchLockAccount"] = 2;
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

            EditAccountViewModel eavModel = new EditAccountViewModel();
            eavModel.Id = Applicationuser.Id;
            eavModel.FullName = Applicationuser.FullName;
            eavModel.Email = Applicationuser.Email;
            eavModel.PhoneNumber = Applicationuser.PhoneNumber;
            eavModel.UserName = Applicationuser.UserName;

            
            return View(eavModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount([Bind(Include = "Id,FullName,UserName,Email,PhoneNumber")] Models.ApplicationUser Applicationuser)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(Applicationuser.Id);
                user.FullName = Applicationuser.FullName;
                user.Email = Applicationuser.Email;
                user.PhoneNumber = Applicationuser.PhoneNumber;
                user.UserName = Applicationuser.UserName;

                //Applicationuser = user;

                UserManager.Update(user);
                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();
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
            Applicationuser.status = "inactive";        
            db.Entry(Applicationuser).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Message"] = "Account Deleted";
            return RedirectToAction("Index");
        }

        // GET: /Admin/BackupAccount
        public ActionResult AdminBackup()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });
            return View();
        }

        public ActionResult SendEmailReminders()
        {
            ICollection<ApplicationUser> expiringAccounts = ((ApplicationUserDataGateway)ApplicationUserGateway).getExpiringAccounts();
            int numOfDays;
            foreach (var act in expiringAccounts)
            {
                numOfDays = Convert.ToInt32((DateTime.Now - act.lastChangedPWd).TotalDays);
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("azure_666521c32094e0a664354efe513747e6@azure.com", "Student Management System");
                    mail.To.Add("lim.m3ixi@gmail.com");
                    mail.Body = "Dear User (" + act.Email + "), <br><br>";
                    if (numOfDays >= 80 && numOfDays < 90)
                    {
                        mail.Subject = "SMS Password Expiring";
                        mail.Body += "<b>Your password is expiring in " + (90 - numOfDays) + " days.</b><br>";
                        mail.Body += "Please change your password soon to prevent your account from getting locked.<br>";
                    }
                    else if (numOfDays == 90)
                    {
                        mail.Subject = "SMS Password Expiring";
                        mail.Body += "<b>Your password is expiring today.</b><br>";
                        mail.Body += "Please change your password <b>immediately</b> to prevent it from getting locked.<br>";
                    }
                    else
                    {
                        mail.Subject = "SMS Password Expired";
                        mail.Body += "<b>Your password has already expired.</b><br>";
                        mail.Body += "Please change your password <b>immediately</b> or else it will be locked in " + (100 - numOfDays) + " days<br>";
                    }

                    mail.Body += "<br>Regards, <br>SMS Admin";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.sendgrid.net", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("azure_666521c32094e0a664354efe513747e6@azure.com", "Password1!");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }

            TempData["SearchLockAccount"] = 1;

            return RedirectToAction("SearchLockAccount");
        }

        //Reset Password
        public ActionResult ResetPassword(string id)
        {

            String newPassword = GenerateRandomPassword(10);
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
            Models.ApplicationUser Applicationuser = db.Users.Find(id);
            if (ModelState.IsValid)
            {
                Applicationuser.PasswordHash = hashedNewPassword;
                Applicationuser.lastChangedPWd = DateTime.Now;
                db.Entry(Applicationuser).State = EntityState.Modified;
                db.SaveChanges();
                string emailid = Applicationuser.Email;
                string subject = "EduTech - Password Reset";
                string body = "Your password has been reset. The new password is " + newPassword;
                try
                {
                    SendEMail(emailid, subject, body);
                    TempData["Message"] = "Mail Sent.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error occured while sending email." + ex.Message;
                }
                return RedirectToAction("SearchAccountParticulars");
            }

            return RedirectToAction("Index");
        }
        //Generate Password
        private string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
        //send email
        private void SendEMail(string emailid, string subject, string body)
        {
            String sender = "edutech3104@gmail.com";
            String password = "4013hceTudE";
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(sender, password);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(sender);
            msg.To.Add(new MailAddress(emailid));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }
    }
}