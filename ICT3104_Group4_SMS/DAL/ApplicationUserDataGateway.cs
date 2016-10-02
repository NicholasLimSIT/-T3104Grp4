using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class ApplicationUserDataGateway : DataGateway<ApplicationUser>
    {
        IEnumerable<ApplicationUser> users;


        public ApplicationUserDataGateway()
        {
            this.data = db.Set<ApplicationUser>();
        }
        public List<string[]> searchStudent(string name)
        {
            if (name.Length > 5){
                name = name.Substring(0, 3);
            }
           
            List<string[]> UserList;
            UserList = new List<string[]>();
            var context = new SmsContext();
            users = data.SqlQuery("SELECT * From dbo.AspNetUsers WHERE UserName LIKE '%" + name + "%'").ToList();

            var role = (from r in db.Roles where r.Name.Contains("Student") select r).FirstOrDefault();
            var user = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            
            foreach (var item in users)
            {
                if (user.Find(x => x.Id == item.Id) != null)
                {
                    // User is in the Admin Role
                    string[] listString = new string[4];
                    listString[0] = item.Id.ToString();
                    listString[1] = item.UserName;
                    listString[2] = item.Email;
                    listString[3] = item.PhoneNumber;
                    UserList.Add(listString);
                }
            }
          
            return UserList;

        }
    }
}