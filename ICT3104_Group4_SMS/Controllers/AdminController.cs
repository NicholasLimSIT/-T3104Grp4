using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ICT3104_Group4_SMS.Controllers
{

   //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private SmsContext db = new SmsContext();
        private ApplicationUserManager _userManager;

        internal IDataGateway<ApplicationUser> ApplicationUserGateway;

        public AdminController()
        {
            ApplicationUserGateway = new ApplicationUserDataGateway();
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
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRole);
                }

            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Admin/EditAccount
        public ActionResult EditAccount()
        {
            return View();
        }

        // GET: /Admin/DeleteAccount
        public ActionResult DeleteAccount()
        {
            return View();
        }

        // GET: /Admin/BackupAccount
        public ActionResult AdminBackup()
        {
            return View();
        }
    }
}