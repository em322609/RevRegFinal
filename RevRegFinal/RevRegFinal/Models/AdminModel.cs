using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RevRegFinal.Models
{
    public class AdminModel : StudentModel
    {
        private static AdminModel instance;
        private AdminModel()
        {

        }

        public static AdminModel GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdminModel();
                }
                return instance;
            }
        }
        

        public void setPassword(string password)
        {
            this.Password = password;
        }

        public void SetClosedFlag(CourseModel c)
        {
            c.isClosed = 1;
        }

        private static bool CloseCourse(CourseModel thisCourseToClose)
        {
            thisCourseToClose.isClosed = 1;
            Console.WriteLine("Registration is closed for " + thisCourseToClose.Title);
            return true;
        }

        public bool ChangeCourseStatus(CourseModel thisCourseToChange)
        {
            thisCourseToChange.isClosed = 1;
            return true;
        }
    }
}