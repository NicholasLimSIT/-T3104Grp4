using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICT3104_Group4_SMS.DAL;
using ICT3104_Group4_SMS.Models;

namespace ICT3104_Group4_SMS.DAL
{
    public class ModuleGateway : DataGateway<Module>
    {
        // get module name from module id
        public String GetModuleName(int? id)
        {
            return data.Where(m => m.Id == id).Select(m => m.name).Take(1).First().ToString();
        }

        public ICollection<Module> getModulesByIds(int[] moduleIds)
        {
            return data.Where(m => moduleIds.Contains(m.Id)).ToList();
        }
    }
}