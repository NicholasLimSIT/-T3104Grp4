using ICT3104_Group4_SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class ProgrammeDataGateway : DataGateway<Programme>
    {
        IEnumerable<Programme> programme;

        public ProgrammeDataGateway()
        {
            this.data = db.Set<Programme>();
        }

        public List<string[]> GetAllProgrammes()
        {
            List<string[]> ProgrammeList;

            programme = SelectAll();
            ProgrammeList = new List<string[]>();

            foreach (var item in programme)
            {
                string[] listString = new string[2];
                listString[0] = item.Id.ToString();
                listString[1] = item.programmeName;
                ProgrammeList.Add(listString);
            }
            return ProgrammeList;
        }

    
    }
}