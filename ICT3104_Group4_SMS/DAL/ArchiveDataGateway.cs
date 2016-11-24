using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ICT3104_Group4_SMS.DAL
{
    public class ArchiveDataGateway : DataGateway<ArchivedRecord>
    {
        private DataGateway<Grade> GradeGateway = new DataGateway<Grade>();
        private DataGateway<ApplicationUser> UserGateway = new DataGateway<ApplicationUser>();


        public int ArchivedRecord()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            int year = Int32.Parse(DateTime.Now.Year.ToString());
            year = year - 3;
            IEnumerable<ApplicationUser> studentList = UserGateway.data.Where(g => g.year == year.ToString()).ToList();
            foreach (var i in studentList)
            {
                if(UserManager.IsInRole(i.Id, "Student"))
                {
                    // Get student grade
                    ArchivedRecord record = new ArchivedRecord();
                    List<Grade> gradeList = new List<Grade>();

                    var model = from grade in db.Grades
                                where grade.studentId == i.Id
                                select grade;
                    gradeList = model.ToList();
                    
                    foreach (var g in gradeList)
                    {
                        Lecturer_Module ltm = db.Lecturer_Module.Find(g.lecturermoduleId);
                        if (ltm != null)
                        {
                            var module = ltm.Module;
                            record.studentName = i.UserName;
                            record.moduleName = module.name;
                        }
                        record.score = g.score;
                        record.grade = g.grade;
                        if (i.GPA != null && i.encryptionKey != null)
                        {
                            string encrypt = i.encryptionKey;
                            string decrypt = Decrypt(i.GPA, encrypt);
                            record.GPA = decrypt;                           
                        }
                        
                        db.ArchivedRecords.Add(record);
                        db.SaveChanges();                     
                        ApplicationUser user =db.Users.Find(i.Id);
                        if (user != null)
                        {      
                            db.Users.Remove(user);
                            db.SaveChanges(); 
                        }
                    }
                    return 1;
                }
            }
            return 0;
        }
        internal static string Decrypt(string input, string key)
        {

            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);

        }
    }
}