using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;

namespace ICT3104_Group4_SMS.DAL
{
    public class ApplicationUserDataGateway : DataGateway<ApplicationUser>
    {
        IEnumerable<ApplicationUser> users;
        private DataGateway<ApplicationUser> UserGateway = new DataGateway<ApplicationUser>();

        string[] rolesArray = { "Admin", "Student", "Lecturer", "HOD" };
        string[] lockrolesArray = {"Student", "Lecturer", "HOD" };

        public ApplicationUserDataGateway()
        {
            this.data = db.Set<ApplicationUser>();
        }


        //Function to Single Search and specific Role?
        public List<string[]> searchSpecficUser(string name)
        {
            if (name.Length > 5)
            {
                name = name.Substring(0, 3);
            }

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            List<string[]> UserList;
            UserList = new List<string[]>();

            var context = new SmsContext();
            users = data.SqlQuery("SELECT * FROM dbo.AspNetUsers WHERE userName LIKE '%" + name + "%'").ToList();

            foreach( var item in users)
            {
                for(int i = 0; i <= rolesArray.Length-1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, rolesArray[i]);
                    if (role)
                    {
                        string[] listString = new string[5];
                        listString[0] = item.Id.ToString();
                        listString[1] = item.UserName;
                        listString[2] = item.Email;
                        listString[3] = item.PhoneNumber;
                        listString[4] = rolesArray[i];
                        UserList.Add(listString);
                    }
                }
            }
            return UserList;
        }

        //Function to search locked users 
        public List<string[]> searchLockUser(string name)
        {
            if (name.Length > 5)
            {
                name = name.Substring(0, 3);
            }

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            List<string[]> UserList;
            UserList = new List<string[]>();

            var context = new SmsContext();
            users = data.SqlQuery("SELECT * FROM dbo.AspNetUsers WHERE userName LIKE '%" + name + "%'").ToList();

            foreach (var item in users)
            {
                for (int i = 0; i <= lockrolesArray.Length - 1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, lockrolesArray[i]);
                    if (role)
                    {
                        string[] listString = new string[5];
                        listString[0] = item.Id.ToString();
                        listString[1] = item.UserName;
                        listString[2] = item.Email;
                        listString[3] = item.PhoneNumber;
                        listString[4] = lockrolesArray[i];
                        UserList.Add(listString);
                    }
                }
            }
            return UserList;


        }

        //Function to Move Users to "RemovedUser Table"
        public void MovedUser (string id)
        {
            string sql = "Insert into RemovedUsers(Email,Id,PhoneNumber,UserName,year) SELECT Email,Id,PhoneNumber,UserName,year FROM AspNetUsers WHERE id = '" +
                id + "'";
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //IEnumerable<ApplicationUser> user = UserGateway.data.Where
            db.Database.ExecuteSqlCommand(sql);
            db.SaveChanges();
        }







        //Function to return all users
        public List<string[]> searchUsers()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            List<string[]> UserList;
            UserList = new List<string[]>();                     
            var context = new SmsContext();
            users = data.SqlQuery("SELECT * From dbo.AspNetUsers").ToList();           
            foreach (var item in users)
            {
                for (int i = 0; i <= rolesArray.Length-1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, rolesArray[i]);
                    if (role)
                    {
                        string[] listString = new string[6];
                        listString[0] = item.Id.ToString();
                        listString[1] = item.UserName;
                        listString[2] = item.Email;
                        listString[3] = item.PhoneNumber;
                        listString[4] = item.year;
                        listString[5] = rolesArray[i];
                        UserList.Add(listString);
                    }                   
                }  
            }
            return UserList;
        }


        //Function to return all users
        public List<string[]> ListLockUsers()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            List<string[]> UserList;
            UserList = new List<string[]>();

            var context = new SmsContext();
            users = data.SqlQuery("SELECT * From dbo.AspNetUsers").ToList();

            foreach (var item in users)
            {

                for (int i = 0; i <= lockrolesArray.Length - 1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, lockrolesArray[i]);
                    if (role)
                    {
                        string[] listString = new string[6];
                        listString[0] = item.Id.ToString();
                        listString[1] = item.UserName;
                        listString[2] = item.Email;
                        listString[3] = item.PhoneNumber;
                        listString[4] = item.year;
                        listString[5] = lockrolesArray[i];
                        UserList.Add(listString);

                    }

                }



            }

            return UserList;

        }

        //Function to search and return a student
        public List<string[]> searchStudent(string name)
        {
            if (name.Length > 5){
                name = name.Substring(0, 3);
            }
           
            List<string[]> UserList;
            UserList = new List<string[]>();
           
            users = data.SqlQuery("SELECT * From dbo.AspNetUsers WHERE UserName LIKE '%" + name + "%'").ToList();

            var role = (from r in db.Roles where r.Name.Contains("Student") select r).FirstOrDefault();
            var user = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            
            foreach (var item in users)
            {
                if (user.Find(x => x.Id == item.Id) != null)
                {
                    // User is in the Student Role
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






        //Function to search and return a student
        public List<string[]> searchAllStudent()
        {

            List<string[]> UserList;
            UserList = new List<string[]>();
            var context = new SmsContext();
            users = data.SqlQuery("SELECT * From dbo.AspNetUsers").ToList();

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

        //update the status of the account from lock to unlock 
        public void unlockAccount(ApplicationUser user)
        {
            user.lockStatus = "Unlock";
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();


        }


    }
}