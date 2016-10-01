using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ICT3104_Group4_SMS.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class LecturerController : Controller
    {
        private SmsContext db = new SmsContext();
        internal IDataGateway<Programme> ProgrammeGateway;
        internal IDataGateway<Lecturer_Module> Lecturer_ModuleGateway;
        public LecturerController()
        {
            Lecturer_ModuleGateway = new Lecturer_ModuleDataGateway();
            ProgrammeGateway = new ProgrammeDataGateway();
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditStudentParticulars()
        {
            return View();
        }

        public ActionResult GradeAssign()
        {
            return View();
        }

        // GET: Programmes
        public ActionResult ProgrammeIndex()
        {
            return View(db.Programmes.ToList());
        }

        // GET: Programmes/Create
        public ActionResult ProgrammeCreate()
        {
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
            return View(db.Modules.ToList());
        }

       
        // GET: Modules/Create
        public ActionResult ModuleCreate()
        {
            ViewBag.List= ((ProgrammeDataGateway)ProgrammeGateway).GetAllProgrammes();

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
        public ActionResult ModuleEdit([Bind(Include = "Id,name")] Module module)
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
    }

   
}