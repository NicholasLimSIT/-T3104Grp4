using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Data.Entity;

namespace ICT3104_Group4_SMS.Controllers
{
    [Authorize(Roles = "HOD")]
    public class HODController : Controller
    {
        private SmsContext db = new SmsContext();
        internal IDataGateway<Programme> ProgrammeGateway;
        internal IDataGateway<Lecturer_Module> Lecturer_ModuleGateway;
        internal IDataGateway<ApplicationUser> ApplicationUserGateway;
        internal IDataGateway<Grade> GradesGateway;
        internal IDataGateway<Recommendation> RecommendationGateway;
        //internal IDataGateway<Module> ModuleGateway = new DataGateway<Module>();
        private ModuleGateway ModuleGateway = new ModuleGateway();
        private Lecturer_ModuleDataGateway lmDW = new Lecturer_ModuleDataGateway();
        private RecommendationGateway recGateway = new RecommendationGateway();
        private SmsMapper smsMapper = new SmsMapper();
        private GradesGateway ggw = new GradesGateway();

        // check if user has passed 2FA authentication. true = did not pass. false = passed.
        public bool IfUserSkipTwoFA()
        {
            if (Session == null)
                return true;
            else if (Session["Verified"] == null)
                return true;

            return false;
        }

        public HODController()
        {
            Lecturer_ModuleGateway = new Lecturer_ModuleDataGateway();
            ProgrammeGateway = new ProgrammeDataGateway();
            ApplicationUserGateway = new ApplicationUserDataGateway();
            GradesGateway = new GradesGateway();
            RecommendationGateway = new RecommendationGateway();
        }

        

        // GET: HOD/ModerateGrade
        public ActionResult ModerateMark()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            return View(db.Modules.ToList());

        }
       
        // GET: Grades
        public ActionResult ModerateMarkView(int? id, String moduleName, String moduleStatus)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.moduleId = id;
            ViewBag.moduleName = moduleName;
            ViewBag.moduleStatus = moduleStatus;
            if (moduleName == null)
                ViewBag.moduleName = ModuleGateway.GetModuleName(id);

            
            int id2 = id ?? default(int);
            IEnumerable<GradeRecViewModel> gradeWithRecList = smsMapper.GradeWithRec(id2);
            return View(gradeWithRecList);
        }

        // GET: /HOD/RecommendationsView
        public ActionResult RecommendationsView()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            IEnumerable<GradeRecViewModel> moduleGradRecList = smsMapper.ModuleWithGradeAndRec();
            return View(moduleGradRecList);
        }

        public ActionResult RecommendationApprove(int? id)
        {
            recGateway.ApproveRec(id);
            TempData["Reviewed"] = 1;
            return RedirectToAction("RecommendationsView");
        }

        public ActionResult RecommendationReject(int? id)
        {
            recGateway.RejectRec(id);
            TempData["Reviewed"] = 2;
            return RedirectToAction("RecommendationsView");
        }

        [HttpPost]
        public ActionResult FreezeGrade(String moduleId) {
            
            int? modId = Convert.ToInt32(moduleId);
            Module module = ModuleGateway.SelectById(modId);
            module.status = "Frozen";
            module.frozenDateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                ModuleGateway.Update(module);
                return RedirectToAction("ModerateMark");
            }
            return RedirectToAction("Index");

        }

        // GET: Grades
        public ActionResult PublishGrade(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Module module = ModuleGateway.SelectById(id);
            module.status = "Published";
            module.publishDateTime = DateTime.Now;
            ModuleGateway.Update(module);
            int id2 = id ?? default(int);
            
            IEnumerable<GradeRecViewModel> gradeWithRecList = smsMapper.GradeWithRec(id2);
            foreach (var g in gradeWithRecList) {
                IEnumerable<String> gradeList = ggw.SelectGrades(g.GradeItem.studentId);
                Models.ApplicationUser Applicationuser = db.Users.Find(g.GradeItem.studentId);
                double score = 0;
                foreach (var gl in gradeList) {
                    String[] grade = gl.Split(',');
                    if (grade[1].Equals("Error"))
                    { score += 0; }
                    else if (grade[1].Equals("A+"))
                    { score += 5.00 * 5.0; }
                    else if (grade[1].Equals("A"))
                    { score += 5.00 * 5.0; }
                    else if (grade[1].Equals("B+"))
                    { score += 4.5 * 5.0; }
                    else if (grade[1].Equals("B"))
                    { score += 4 * 5.0; }
                    else if (grade[1].Equals("C+"))
                    { score += 3.5 * 5.0; }
                    else if (grade[1].Equals("C"))
                    { score += 3 * 5.0; }
                    else
                    { score += 0; }
                }
                //5 credit unit for all module
                Random rand = new Random();
                double totalgpa = score/ (gradeList.Count()*5);
                String encrypt = Encrypt(totalgpa, rand);

                if (encrypt != null && Applicationuser != null)
                {
                    if (ModelState.IsValid)
                    {
                        String[] gpa_encrypt = encrypt.Split(',');

                        Applicationuser.GPA = gpa_encrypt[0];
                        Applicationuser.encryptionKey = gpa_encrypt[1];
                        db.Entry(Applicationuser).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ModerateMark");
        }

        // GET: Grades
        public ActionResult ModerateAction(String moduleId, String moderationPercentage)
        {
            int? id = Convert.ToInt32(moduleId);



            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            ViewBag.moduleId = moduleId;
            ViewBag.moduleName = ModuleGateway.GetModuleName(id);

            int id2 = id ?? default(int);

            ViewBag.moderationAction = smsMapper.moderateGrade(id2, moderationPercentage);
            return RedirectToAction("ModerateMarkView", new { id = id });
        }

        internal static string Encrypt(double grade, Random random)
        {
            string input = Convert.ToString(grade);
            string key = GenerateKey(16, random);
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            string encryptgpa = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            string result = encryptgpa + "," + key;
            Debug.WriteLine(result);
            return result;
        }

        // GET: /HOD/StudentGPA
        public ActionResult StudentGPA()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            List<Module> moduleList = db.Modules.ToList();
            List<Module> publish = new List<Module>();
            foreach (var ml in moduleList)
            {

                if (ml.status.Equals("Published"))
                {
                    publish.Add(ml);
                    
                }
            }
            List<ApplicationUser> userList = new List<ApplicationUser>();

            foreach (var p in publish)
            {
                IEnumerable<Grade> gradeList = lmDW.selectStudentByModule(p.Id);
                foreach (var gl in gradeList)
                {
                    Models.ApplicationUser Auser = db.Users.Find(gl.studentId);
                    if (Auser != null && Auser.GPA != null)
                    {
                        if (!userList.Contains(Auser))
                        {
                            String gpa = Auser.GPA;
                            String decrypt = Decrypt(gpa, Auser.encryptionKey);
                            Auser.GPA = decrypt;

                            userList.Add(Auser);
                        }
                    }
                }
            }

            return View(userList);

        }


        private static string GenerateKey(int length, Random random)
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
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

            return UTF8Encoding.UTF8.GetString(resultArray);

        }
    }
}