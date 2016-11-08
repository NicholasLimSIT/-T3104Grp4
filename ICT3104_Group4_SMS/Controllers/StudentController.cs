using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace ICT3104_Group4_SMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private GradesGateway ggw = new GradesGateway();
        internal IDataGateway<ApplicationUser> ApplicationUserGateway;
        private SmsContext db = new SmsContext();


        public StudentController()
        {
            ApplicationUserGateway = new ApplicationUserDataGateway();
            
        }
        //View Grade
        public ActionResult ViewGrade()
        {
            var userID = User.Identity.GetUserId();
            ViewBag.ModulesGrades = ggw.SelectGrades(userID);
            Models.ApplicationUser Applicationuser = db.Users.Find(userID);
            if (Applicationuser == null)
            {
                return HttpNotFound();
            }
            string gpa = Applicationuser.GPA;
            if (gpa != null && Applicationuser.encryptionKey!= null)
            {
                string encrypt = Applicationuser.encryptionKey;
                string decrypt = Decrypt(gpa, encrypt);
                ViewBag.GPA = decrypt;
            }
            return View();
        }

        internal static string Decrypt(string input, string key)
        {
            
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            Debug.WriteLine("Hello" + UTF8Encoding.UTF8.GetString(resultArray)); 
            return UTF8Encoding.UTF8.GetString(resultArray);
            
        }

      
    }
}
 