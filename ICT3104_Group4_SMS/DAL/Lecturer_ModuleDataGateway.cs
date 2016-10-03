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
    }
}