using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class RecommendationGateway : DataGateway<Recommendation>
    {        
        public ICollection<Recommendation> GetRecByGradeIds(int[] gradeIds)
        {
            ICollection<Recommendation> model = new List<Recommendation>();
            model = data.Where(r => gradeIds.Contains(r.gradeId)).ToList();
            return model;
        }

        public ICollection<Recommendation> GetPendingRecs()
        {
            return data.Where(r => r.status.Equals("Pending")).ToList();
        }

        public void ApproveRec(int? id)
        {
            Recommendation recItem = SelectById(id);
            recItem.status = "Approved";
            Update(recItem);
        }
    }
}