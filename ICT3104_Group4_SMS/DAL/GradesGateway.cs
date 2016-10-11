using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;

namespace ICT3104_Group4_SMS.DAL
{
    public class GradesGateway : DataGateway<Grade>
    {
        private DataGateway<Lecturer_Module> LMGateway = new DataGateway<Lecturer_Module>();
        private DataGateway<Module> ModuleGateway = new DataGateway<Module>();
        public IEnumerable<String> SelectGrades(String id)
        {
            var model = from grade in db.Grades
                        where grade.studentId == id
                        select grade;
            List<Grade> gradeList = model.ToList();

            //get the module id in lecturer_module table
            List<Lecturer_Module> LMList = new List<Lecturer_Module>();
            foreach (var grade in gradeList)
            {

                LMList.Add(LMGateway.SelectById(grade.lecturermoduleId));
            }

            // get the module detail id in module table
            List<Module> moduleList = new List<Module>();
            foreach (var lm in LMList)
            {
                moduleList.Add(ModuleGateway.SelectById(lm.moduleId));

            }

            // get list of module name
            List<String> moduleNameList = new List<String>();
            foreach (var ml in moduleList)
            {
                moduleNameList.Add(ml.name);
            }
            

            List<String> scoreList = new List<string>();
            foreach (var gl in gradeList) {
                string score = Convert.ToString(gl.score);
                scoreList.Add(score);
            }


            //put the grade and module name together.
            List<String> gradeNameList = new List<String>();
            for (int i = 0; i < scoreList.Count(); i++)
            {
                gradeNameList.Add(moduleNameList[i] + "," +scoreList[i]);
            }

            return gradeNameList;
        }

        public ICollection<Grade> GetGradesByLecMod(int[] lecModIds)
        {
            ICollection<Grade> gradeList = new List<Grade>();
            gradeList = data.Where(g => lecModIds.Contains(g.lecturermoduleId)).ToList();

            return gradeList;
        }

        internal IEnumerable<Grade> GetGradesByRec(int[] gradeIds)
        {
            return data.Where(g => gradeIds.Contains(g.Id)).ToList();
        }
    }
}