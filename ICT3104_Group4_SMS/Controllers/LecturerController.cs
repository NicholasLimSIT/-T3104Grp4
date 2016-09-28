using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ICT3104_Group4_SMS.Controllers
{
    public class LecturerController : Controller
    {
        private SmsContext db = new SmsContext();
        internal IDataGateway<Class> classDataGateway;

        public LecturerController()
        {
            classDataGateway = new ClassDataGateway();

        }
        // GET: Lecturer
        public ActionResult Index()
        {
            return View();
        }
        // GET: Classes
        public ActionResult ClassIndex()
        {
            return View(db.Classes.ToList());
        }
        // GET: Classes/Create
        public ActionResult ClassCreate()
        {
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClassCreate([Bind(Include = "Id,name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Add(@class);
                db.SaveChanges();
                return RedirectToAction("ClassIndex");
            }

            return View(@class);
        }

        // GET: Classes/Edit/5
        public ActionResult ClassEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClassEdit([Bind(Include = "Id,name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@class).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ClassIndex");
            }
            return View(@class);
        }

        // GET: Classes/Delete/5
        public ActionResult ClassDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("ClassDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
            db.SaveChanges();
            return RedirectToAction("ClassIndex");
        }
    }
}