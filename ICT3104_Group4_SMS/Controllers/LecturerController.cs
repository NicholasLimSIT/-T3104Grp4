using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ICT3104_Group4_SMS.Controllers
{
    [Authorize(Roles = "Lecturer,Admin")]
    public class LecturerController : Controller
    {
        private SmsContext db = new SmsContext();
        internal IDataGateway<Programme> ProgrammeGateway;
        internal IDataGateway<Lecturer_Module> Lecturer_ModuleGateway;
        internal IDataGateway<ApplicationUser> ApplicationUserGateway;
        internal IDataGateway<Grade> GradesGateway;
        internal IDataGateway<Recommendation> RecommendationGateway;
        private ApplicationUserManager _userManager;

        //internal IDataGateway<Module> ModuleGateway = new DataGateway<Module>();
        private ModuleGateway ModuleGateway = new ModuleGateway();
        private Lecturer_ModuleDataGateway lmDW = new Lecturer_ModuleDataGateway();

        private SmsMapper smsMapper = new SmsMapper();

        // check if user has passed 2FA authentication. true = did not pass. false = passed.
        public bool IfUserSkipTwoFA()
        {
            if (Session == null)
                return true;
            else if (Session["Verified"] == null)
                return true;

            return false;
        }

        public LecturerController()
        {
            Lecturer_ModuleGateway = new Lecturer_ModuleDataGateway();
            ProgrammeGateway = new ProgrammeDataGateway();
            ApplicationUserGateway = new ApplicationUserDataGateway();
            GradesGateway = new GradesGateway();
            RecommendationGateway = new RecommendationGateway();
        }

        public LecturerController(ApplicationUserManager userManager)
        {
            Lecturer_ModuleGateway = new Lecturer_ModuleDataGateway();
            ProgrammeGateway = new ProgrammeDataGateway();
            ApplicationUserGateway = new ApplicationUserDataGateway();
            GradesGateway = new GradesGateway();
            RecommendationGateway = new RecommendationGateway();

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

        [HttpGet]
        public ActionResult SearchStudentParticulars(string name)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (name != null)
            {
                ViewBag.List = ((ApplicationUserDataGateway)ApplicationUserGateway).searchStudent(name);
            }
            else
            {
                ViewBag.List = ((ApplicationUserDataGateway)ApplicationUserGateway).searchStudent("");
            }
            return View();
        }

        // GET: /Lecturer/EditStudentParticulars
        public ActionResult EditStudentParticulars(string id)
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


        // POST: Programmes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudentParticulars([Bind(Include = "Id,FullName,UserName,Email,PhoneNumber")] Models.ApplicationUser Applicationuser)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(Applicationuser.Id);
                user.FullName = Applicationuser.FullName;
                user.Email = Applicationuser.Email;
                user.PhoneNumber = Applicationuser.PhoneNumber;
                user.UserName = Applicationuser.UserName;
                UserManager.Update(user);

                //db.Entry(Applicationuser).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("SearchStudentParticulars");
            }
            return View(Applicationuser);
        }

        // GET: /Lecturer/DeleteStudent
        public ActionResult DeleteStudent(string id)
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

        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Models.ApplicationUser Applicationuser = db.Users.Find(id);
            db.Users.Remove(Applicationuser);
            db.SaveChanges();
            return RedirectToAction("SearchStudentParticulars");
        }

        // GET: /Lecturer/ModuleTeach
        public ActionResult ModuleTeach()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            var userID = User.Identity.GetUserId();
            return View(lmDW.selectModuleByLecturer(userID));
        }


        // GET: /Lecturer/GradeAssign
        public ActionResult GradeAssign(int? id)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // lmDW.getModuleStudent(id);
            IEnumerable<Grade> gradeList = lmDW.selectStudentByModule(id);

            if (gradeList.Count() == 0)
            {
                var module1 = ModuleGateway.SelectById(id);
                ViewBag.selectedModule = module1;
                return View(gradeList);
            }
            List<Grade> gList = new List<Grade>(); ;
            List<String> studNameList = new List<string>();
            if (gradeList.Count() != 0)
            {

                foreach (var g in gradeList)
                {
                    var user = (from u in db.Users
                                where u.Id == (g.studentId)
                                select u).FirstOrDefault();
                    if (user != null)
                    {
                        studNameList.Add(user.FullName);
                        gList.Add(g);
                    }

                }
            }

            var module = ModuleGateway.SelectById(id);
            ViewBag.selectedModule = module;
            ViewBag.nameList = studNameList;
            return View(gList);

        }

        [HttpPost]
        public ActionResult GradeAssign(List<Grade> list, string moduleId)
        {
            int? modId = Convert.ToInt32(moduleId);
            if (ModelState.IsValid && list != null)
            {

                foreach (var i in list)
                {
                    var c = db.Grades.Where(a => a.Id.Equals(i.Id)).FirstOrDefault();
                    if (c != null)
                    {
                        c.lecturermoduleId = i.lecturermoduleId;
                        c.score = i.score;
                        c.studentId = i.studentId;
                    }

                }
                db.SaveChanges();
                ViewBag.Message = "Successfully Updated";

                //return View("GradeAssign(" + modId+")");
                return RedirectToAction("GradeAssign/" + modId);

            }
            else
            {
                ViewBag.Message = "Failed! Please try again";
                return RedirectToAction("ModuleTeach");
                //return View(list);
            }

        }

        // GET: Grades
        public ActionResult GradesView(int? id, String moduleName)
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
            if (moduleName == null)
                ViewBag.moduleName = ModuleGateway.GetModuleName(id);

            ViewBag.moduleStatus = ModuleGateway.GetModuleStatus(id);

            int id2 = id ?? default(int);
            IEnumerable<GradeRecViewModel> gradeWithRecList = smsMapper.GradeWithRec(id2);
            return View(gradeWithRecList);
        }

        // GET: Programmes
        public ActionResult ProgrammeIndex()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            return View(db.Programmes.ToList());
        }

        // GET: Programmes/Create
        public ActionResult ProgrammeCreate()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            return View();
        }

        // POST: Programmes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProgrammeCreate([Bind(Include = "Id,programmeName")] Programme programme)
        {
            if (ModelState.IsValid)
            {
                programme.lecturerId = User.Identity.GetUserId();
                db.Programmes.Add(programme);
                db.SaveChanges();
                return RedirectToAction("ProgrammeIndex");
            }

            return View(programme);
        }

        // GET: Programmes/Edit/5
        public ActionResult ProgrammeEdit(int? id)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programmes.Find(id);
            if (programme == null)
            {
                return HttpNotFound();
            }
            return View(programme);
        }

        // POST: Programmes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProgrammeEdit([Bind(Include = "Id,programmeName")] Programme programme)
        {
            if (ModelState.IsValid)
            {

                db.Entry(programme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProgrammeIndex");
            }
            return View(programme);
        }

        // GET: Programmes/Delete/5
        public ActionResult ProgrammeDelete(int? id)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programmes.Find(id);
            if (programme == null)
            {
                return HttpNotFound();
            }
            return View(programme);
        }

        // POST: Programmes/Delete/5
        [HttpPost, ActionName("ProgrammeDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programme programme = db.Programmes.Find(id);
            db.Programmes.Remove(programme);
            db.SaveChanges();
            return RedirectToAction("ProgrammeIndex");
        }


        // GET: Modules
        public ActionResult ModuleIndex()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            return View(db.Modules.ToList());
        }


        // GET: Modules/Create
        public ActionResult ModuleCreate()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            ViewBag.List = ((ProgrammeDataGateway)ProgrammeGateway).GetAllProgrammes();

            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModuleCreate([Bind(Include = "Id,name,year")] Module module)
        {
            if (ModelState.IsValid)
            {

                module.year = Int32.Parse(DateTime.Now.Year.ToString());
                module.status = "Created";
                db.Modules.Add(module);
                db.SaveChanges();
                //get the id of the inserted module
                int id = module.Id;
                Lecturer_Module lmModel = new Lecturer_Module();
                lmModel.lecturerId = User.Identity.GetUserId();
                lmModel.moduleId = id;
                //insert to Lecturer_Model
                ((Lecturer_ModuleDataGateway)Lecturer_ModuleGateway).Insert(lmModel);
                return RedirectToAction("ModuleIndex");
            }

            return View(module);
        }

        // GET: Modules/Edit/5
        public ActionResult ModuleEdit(int? id)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModuleEdit([Bind(Include = "Id,name, frozenDateTime, publishDateTime, year")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ModuleIndex");
            }
            return View(module);
        }

        // GET: Modules/Delete/5
        public ActionResult ModuleDelete(int? id)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("ModuleDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult MoudleDeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("ModuleIndex");
        }

        // GET: Recommendations/Create
        public ActionResult RecommendationCreate(int? id, string name)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            Grade gradeItem = GradesGateway.SelectById(id);
            ViewBag.gradeItem = gradeItem;
            ViewBag.moduleId = lmDW.GetModuleIdFromLecModId(gradeItem.lecturermoduleId);
            ViewBag.name = name;

            return View();
        }

        // POST: Recommendations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecommendationCreate([Bind(Include = "Id,gradeId, recommendation, status")] Recommendation rec)
        {
            if (ModelState.IsValid)
            {
                rec.lecturerId = User.Identity.GetUserId();
                RecommendationGateway.Insert(rec);
                TempData["Success"] = 1;            // for success message on recommendationedit
                return RedirectToAction("RecommendationEdit", new { id = rec.Id, gradeId = rec.gradeId });
            }

            return View(rec);
        }

        // GET: Recommendations/Edit/5
        public ActionResult RecommendationEdit(int? id, int gradeId)
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recommendation rec = RecommendationGateway.SelectById(id);

            if (rec == null)
            {
                return HttpNotFound();
            }
            
            Grade gradeItem = GradesGateway.SelectById(gradeId);
            var student = UserManager.FindById(gradeItem.studentId);

            ViewBag.gradeItem = gradeItem;
            ViewBag.moduleId = lmDW.GetModuleIdFromLecModId(gradeItem.lecturermoduleId);
            ViewBag.name = student.FullName;
            return View(rec);
        }

        // POST: Recommendations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecommendationEdit([Bind(Include = "Id, gradeId, recommendation, status")] Recommendation rec)
        {
            if (ModelState.IsValid)
            {
                rec.lecturerId = User.Identity.GetUserId();
                RecommendationGateway.Update(rec);
                TempData["Success"] = 2;                    // for success message on recommendationedit
                return RedirectToAction("RecommendationEdit", new { gradeId = rec.gradeId });
            }
            return View(rec);
        }


        // GET: StudentEnrolment
        public ActionResult StudentEnrolment()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            var userID = User.Identity.GetUserId();
            return View(lmDW.selectModuleByLecturer(userID));
        }

        // GET: Grades
        public ActionResult StudentEnrolView(int? id, String moduleName)
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

            IEnumerable<Grade> gradeList = lmDW.selectStudentByModule(id);
            List<string[]> studentList = ((ApplicationUserDataGateway)ApplicationUserGateway).searchAllStudent();
            List<string[]> studentNotInModuleList = new List<string[]>();

            foreach (var student in studentList)
            {
                bool found = false;
                foreach (var grade in gradeList)
                {
                    if (student.ElementAt(0).Equals(grade.studentId))
                    {
                        found = true;
                    }
                }
                if (found == false)
                {
                    studentNotInModuleList.Add(student);
                }
            }
            ViewBag.ListStudentNotInModule = studentNotInModuleList;
            ViewBag.selectedEnrolModule = moduleName;

            return View();
        }

        // GET: Grades
        public ActionResult StudentEnrol(string studentId, string moduleName)
        {
            if (studentId.Length != 0)
            {
                Grade newGrade = new Grade();
                newGrade.studentId = studentId;
                IEnumerable<Module> modList = ModuleGateway.SelectAll();

                foreach (var i in modList)
                {
                    if (i.name.Equals(moduleName))
                    {
                        IEnumerable<Lecturer_Module> lecModList = Lecturer_ModuleGateway.SelectAll();
                        foreach (var j in lecModList)
                        {
                            if (j.moduleId == i.Id)
                            {
                                newGrade.lecturermoduleId = j.Id;
                            }
                        }
                    }
                }

                GradesGateway.Insert(newGrade);
            }



            return RedirectToAction("StudentEnrolment");
        }

        // GET: Grades
        public ActionResult LockGrade(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Module module = ModuleGateway.SelectById(id);
            module.status = "Locked";
            ModuleGateway.Update(module);
            ViewBag.moduleStatus = "Locked";

            return View();
        }
        public ActionResult StudentGPA()
        {
            // check if user has passed 2FA verification. if no, redirect to login page
            if (IfUserSkipTwoFA())
                return RedirectToAction("LoginNonStudent", "Account", new { ReturnUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri });

            var userID = User.Identity.GetUserId();
            IEnumerable<Module> LecModule = lmDW.selectModuleByLecturer(userID);
            List<Module> moduleList = db.Modules.ToList();
            List<Module> publish = new List<Module>();
            foreach (var ml in moduleList)
            {

                if (ml.status.Equals("Published"))
                {

                    if (LecModule.Any(x => x.Id == ml.Id))
                    {
                        publish.Add(ml);
                    }
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




