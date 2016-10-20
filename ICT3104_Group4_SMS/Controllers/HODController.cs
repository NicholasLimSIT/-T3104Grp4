using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ICT3104_Group4_SMS.Controllers
{
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

        public HODController()
        {
            Lecturer_ModuleGateway = new Lecturer_ModuleDataGateway();
            ProgrammeGateway = new ProgrammeDataGateway();
            ApplicationUserGateway = new ApplicationUserDataGateway();
            GradesGateway = new GradesGateway();
            RecommendationGateway = new RecommendationGateway();
        }

        // GET: HOD
        public ActionResult Index()
        {
            return View();
         
        }

        // GET: HOD/ModerateGrade
        public ActionResult ModerateMark()
        {
            return View(db.Modules.ToList());

        }
        [HttpGet]
        public ActionResult GraduateStudentsView()
        {
            return View(db.Users.ToList());

        }
        public ActionResult GraduateStudentsView(int id)
        {
            return View(db.Users.ToList());

        }
        // GET: Grades
        public ActionResult ModerateMarkView(int? id, String moduleName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.moduleId = id;
            ViewBag.moduleName = moduleName;
            if (moduleName == null)
                ViewBag.moduleName = ModuleGateway.GetModuleName(id);

            int id2 = id ?? default(int);
            IEnumerable<GradeRecViewModel> gradeWithRecList = smsMapper.GradeWithRec(id2);
            return View(gradeWithRecList);
        }

        // GET: Recommendations
        public ActionResult RecommendationsView()
        {
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
            module.status = "Freeze";
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
            return RedirectToAction("ModerateMark");
        }

    }
}