using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class RecommendationDataGateway : DataGateway<Recommendation>
    {
        IEnumerable<Recommendation> recommendations;

        public RecommendationDataGateway()
        {
            this.data = db.Set<Recommendation>();
        }    
    }
}