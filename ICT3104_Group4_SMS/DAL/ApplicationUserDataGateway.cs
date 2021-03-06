﻿using ICT3104_Group4_SMS.Models;
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
            users = data.SqlQuery("SELECT * FROM dbo.AspNetUsers WHERE FullName LIKE '%" + name + "%'").ToList();

            foreach( var item in users)
            {
                for(int i = 0; i <= rolesArray.Length-1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, rolesArray[i]);
                    if (role && item.status != "inactive")
                    {
                        string[] listString = new string[7];
                        listString[0] = item.Id.ToString();
                        listString[1] = item.UserName;
                        listString[2] = item.Email;
                        listString[3] = item.PhoneNumber;
                        listString[4] = rolesArray[i];
                        listString[6] = item.FullName;
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
            users = data.SqlQuery("SELECT * FROM dbo.AspNetUsers WHERE lockStatus = 'Lock' AND userName LIKE '%" + name + "%'").ToList();

            foreach (var item in users)
            {
                for (int i = 0; i <= lockrolesArray.Length - 1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, lockrolesArray[i]);
                    if (role && item.status != "inactive")
                    {
                        string[] listString = new string[7];
                        listString[0] = item.Id.ToString();
                        listString[1] = item.UserName;
                        listString[2] = item.Email;
                        listString[3] = item.PhoneNumber;
                        listString[4] = lockrolesArray[i];
                        listString[6] = item.FullName;
                        UserList.Add(listString);
                    }
                }
            }
            return UserList;


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
                    if (role && item.status!="inactive")
                    {
                        string[] listString = new string[7];
                        listString[0] = item.Id.ToString();
                        listString[1] = item.UserName;
                        listString[2] = item.Email;
                        listString[3] = item.PhoneNumber;
                        listString[4] = item.year;
                        listString[5] = rolesArray[i];
                        listString[6] = item.FullName;
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
            users = data.SqlQuery("SELECT * From dbo.AspNetUsers WHERE lockStatus = 'Lock'").ToList();

            foreach (var item in users)
            {

                for (int i = 0; i <= lockrolesArray.Length - 1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, lockrolesArray[i]);
                    if (role)
                    {
                        string[] listString = new string[7];
                        listString[0] = item.Id.ToString();
                        listString[1] = item.UserName;
                        listString[2] = item.Email;
                        listString[3] = item.PhoneNumber;
                        listString[4] = item.year;
                        listString[5] = lockrolesArray[i];
                        listString[6] = item.FullName;
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
                    string[] listString = new string[5];
                    listString[0] = item.Id.ToString();
                    listString[1] = item.UserName;
                    listString[2] = item.Email;
                    listString[3] = item.PhoneNumber;
                    listString[4] = item.FullName;
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
                    string[] listString = new string[5];
                    listString[0] = item.Id.ToString();
                    listString[1] = item.UserName;
                    listString[2] = item.Email;
                    listString[3] = item.PhoneNumber;
                    listString[4] = item.FullName;
                    UserList.Add(listString);
                }
            }

            return UserList;

        }


        //Function to update the  expire user's status to Lock
        public void lockExpireUsers()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var context = new SmsContext();
            users = data.SqlQuery("SELECT * From dbo.AspNetUsers").ToList();
            foreach (var item in users)
            {
                for (int i = 0; i <= lockrolesArray.Length - 1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, lockrolesArray[i]);
                    if (role)
                    {
                        double totaldays = (DateTime.Now - item.lastChangedPWd ).TotalDays;
                        if (totaldays >= 100)
                        {
                            item.lockStatus = "Lock";
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                   
                }
            }
        }

        public ICollection<ApplicationUser> getExpiringAccounts()
        {
            ICollection<ApplicationUser> expiringAccounts = new List<ApplicationUser>();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var context = new SmsContext();
            users = data.SqlQuery("SELECT * From dbo.AspNetUsers").ToList();
            foreach (var item in users)
            {
                for (int i = 0; i <= lockrolesArray.Length - 1; i++)
                {
                    bool role = UserManager.IsInRole(item.Id, lockrolesArray[i]);
                    if (role)
                    {
                        double totaldays = (DateTime.Now - item.lastChangedPWd).TotalDays;
                        if (totaldays >= 80)
                        {
                            expiringAccounts.Add(item);
                        }
                    }
                }
            }
            return expiringAccounts;
        }

        public ICollection<ApplicationUser> getNamesById(string[] studentIds)
        {
            var allUsers = data.Select(u => u.Id);
            return data.Where(u => studentIds.Contains(u.Id)).ToList();
        }
    }
}