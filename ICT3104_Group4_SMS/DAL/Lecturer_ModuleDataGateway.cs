using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class Lecturer_ModuleDataGateway : DataGateway<Lecturer_Module>
    {
        private DataGateway<Module> ModuleGateway = new DataGateway<Module>();
        private DataGateway<Lecturer_Module> Lecturer_ModuleGateway = new DataGateway<Lecturer_Module>();
        private DataGateway<Grade> GradeGateway = new DataGateway<Grade>();

        public IEnumerable<Module> selectModuleByLecturer(string lecId)
        {
            //Get list of modules id taught by the lecturer
            var model = from lm in db.Lecturer_Module
                        where lm.lecturerId == lecId
                        select lm;
            List<Lecturer_Module> lmList = model.ToList();

            List<Module> moduleNameList = new List<Module>();

            //get Module Name taught by lecturer
            foreach (var lm in lmList)
            {
                moduleNameList.Add(ModuleGateway.SelectById(lm.moduleId));
            }


            return moduleNameList;

        }


        public IEnumerable<Grade> selectStudentByModule(int? mId)
        {
            //Get lecturer_module id taught by module id
            List<int> lmList = new List<int>();

            var model1 = from lm in db.Lecturer_Module
                         where lm.moduleId == mId
                         select lm.Id;
            lmList = model1.ToList();


            //Get list of student in the module
            List<Grade> gradeList = new List<Grade>();
            foreach (var lm in lmList)
            {
                var model2 = from g in db.Grades
                             where g.lecturermoduleId == lm
                             select g;
                gradeList = model2.ToList();
            }


            /*   List<String> studentList = new List<string>();

               foreach (var gl in gradeList) {
                   var model3 = from u in db.Users
                                where u.Id == gl.studentId
                                select u.Email;

               }
             */
            return gradeList;

        }

        public void getModuleStudent(int? modId)
        {
            // get role id
            var role = (from r in db.Roles where r.Name.Contains("Student") select r).FirstOrDefault();
            //get  list student
            var user = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            // get student Id 
            List<String> userId = new List<String>();
            foreach (var u in user)
            {
                userId.Add(u.Id);
            }

            var grade = GradeGateway.SelectAll();
            var lModuleId = (from lm in db.Lecturer_Module where lm.moduleId == modId select lm.Id).FirstOrDefault();

            //get  curent student id in the module
            List<String> userModule = new List<String>();
            foreach (var g in grade) {
                if (g.lecturermoduleId == lModuleId) {
                    userModule.Add(g.studentId);
                }
            }
            
            //check if student in module if not add to the module
            for (int i = 0; i < userId.Count(); i++) {

                if (!userModule.Contains(userId[i])) {
                    Grade userGrade = new Grade();
                    userGrade.lecturermoduleId = lModuleId;
                    userGrade.studentId = userId[i];
                    userGrade.score = 0.0;
                    GradeGateway.Insert(userGrade);
                }
            }

        }
    }
}