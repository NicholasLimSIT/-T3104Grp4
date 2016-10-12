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
          
            // Get publish modules id.
            List<int> publishList = new List<int>();

            IEnumerable<Module> moduleList = ModuleGateway.SelectAll();
            foreach (var m in moduleList) {
                if (m.status == "Published") {

                    publishList.Add(m.Id);
                }

            }

            // Select lm id for published module id
            List<int> lmList = new List<int>();
            IEnumerable<Lecturer_Module> lmEm = LMGateway.SelectAll();
            foreach (var lm in lmEm) {
                if (publishList.Contains(lm.moduleId)) {
                    lmList.Add(lm.Id);
                }
            }

            // Get student grade
            List<Grade> gradeList = new List<Grade>();
            
            var model = from grade in db.Grades
                        where grade.studentId == id
                        select grade;
            gradeList = model.ToList();

            // Get student published grade
            List<Grade> pgradeList = new List<Grade>();
            foreach (var g in gradeList) {

                if (lmList.Contains(g.lecturermoduleId)) {
                    pgradeList.Add(g);
                }
            }

            // Get publish module name
            List<String> pModuleNameList = new List<string>(); 
            foreach (var pm in publishList) {

                pModuleNameList.Add(ModuleGateway.SelectById(pm).name);
            }

            //Convert publish grade to String (A+ , A- etc)
            List<String> scoreList = new List<string>();
            foreach (var gl in pgradeList)
            {
                string score = "-";
                if (gl.score > 100.0)
                { score = "Error"; }
                else if (gl.score > 85.0)
                { score = "A+"; }
                else if (gl.score > 80.0)
                { score = "A"; }
                else if(gl.score > 75.0)
                { score = "B+"; }
                else if (gl.score > 70.0)
                { score = "B"; }
                else if (gl.score > 50.0)
                { score = "C+"; }
                else if (gl.score > 30.0)
                { score = "C"; }
                else
                { score = "F"; }
            
                scoreList.Add(score);
            }
            //put the grade and module name together.
            List<String> gradeNameList = new List<String>();
            for (int i = 0; i < scoreList.Count(); i++)
            {
                gradeNameList.Add(pModuleNameList[i] + "," + scoreList[i]);
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