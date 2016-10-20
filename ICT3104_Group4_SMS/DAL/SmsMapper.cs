using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class SmsMapper
    {
        private Lecturer_ModuleDataGateway lmGateway = new Lecturer_ModuleDataGateway();
        private GradesGateway gradeGateway = new GradesGateway();
        private RecommendationGateway recGateway = new RecommendationGateway();
        private ModuleGateway modGateway = new ModuleGateway();

        // get grades mapped to recommendation for one module
        public ICollection<GradeRecViewModel> GradeWithRec(int moduleId)
        {
            ICollection<GradeRecViewModel> gradeRecList = new List<GradeRecViewModel>();

            // get all lecturermoduleid with the same module id
            int[] lecModIds = lmGateway.GetLecModIdsByModule(moduleId);

            // get all grades that are under this lecturermoduleid
            IEnumerable<Grade> gradeList = gradeGateway.GetGradesByLecMod(lecModIds);
            int[] gradeIds = gradeList.Select(grade => grade.Id).Distinct().ToArray();

            // get all recommendation that are for the selected grades (in gradelist)
            IEnumerable<Recommendation> recList = recGateway.GetRecByGradeIds(gradeIds);

            // a recommendation object to be used when no recommendation is found for that grade
            Recommendation noRec = new Recommendation();
            noRec.recommendation = "-";
            noRec.status = "-";

            // mapping of grade to recommendation
            // each graderecviewmodel has a grade and a recommendation
            foreach (var grade in gradeList)
            {
                GradeRecViewModel tempModel = new GradeRecViewModel();
                tempModel.GradeItem = grade;
                // select the recommendation that is for current grade 
                IEnumerable<Recommendation> tempRecItem = recList.Where(rec => rec.gradeId == grade.Id).ToList();
                if (tempRecItem.Count() != 0)
                    tempModel.RecItem = tempRecItem.First();
                else        // no recommendation found, use the object created for no recommendation
                    tempModel.RecItem = noRec;
                gradeRecList.Add(tempModel);
            }

            // list of mapped grades and recommendation  
            return gradeRecList;
        }

        public ICollection<GradeRecViewModel> ModuleWithGradeAndRec()
        {
            ICollection<GradeRecViewModel> modGradeRecList = new List<GradeRecViewModel>();

            IEnumerable<Recommendation> recList = recGateway.GetPendingRecs();
            int[] gradeIds = recList.Select(rec => rec.gradeId).Distinct().ToArray();

            IEnumerable<Grade> gradeList = gradeGateway.GetGradesByRec(gradeIds);
            int[] lecModIds = gradeList.Select(grade => grade.lecturermoduleId).Distinct().ToArray();

            IEnumerable<Lecturer_Module> lecmodList = lmGateway.GetLecModsFromLecModIds(lecModIds);
            int[] moduleIds = lecmodList.Select(lm => lm.moduleId).Distinct().ToArray();

            IEnumerable<Module> modList = modGateway.getModulesByIds(moduleIds);
            
            // mapping of grade to recommendation
            // each graderecviewmodel has a grade and a recommendation
            foreach (var rec in recList)
            {
                GradeRecViewModel tempModel = new GradeRecViewModel();
                // select the recommendation that is for current grade 
                Grade tempGradeItem = gradeList.Where(grade => grade.Id == rec.gradeId).Take(1).First();
                Lecturer_Module tempLecMod = lecmodList.Where(lm => lm.Id == tempGradeItem.lecturermoduleId).Take(1).First();
                String moduleName = modList.Where(m => m.Id == tempLecMod.moduleId).Select(m => m.name).Take(1).First();

                tempModel.RecItem = rec;
                tempModel.GradeItem = tempGradeItem;
                tempModel.ModuleName = moduleName;
                modGradeRecList.Add(tempModel);
            }

            // list of mapped grades and recommendation  
            return modGradeRecList;
        }






    }
}