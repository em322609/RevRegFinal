using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RevRegFinal.Models
{
    public class StudentModel
    {
      
        #region CTOR
        public StudentModel()
        {
        }

        public string major;
        private bool isFullTime;
        private Dictionary<string, CourseModel> schedule = new Dictionary<string, CourseModel>();
        
        public StudentModel(string fullname, string password, string email, int id, string major)
        {
            this.FullName = fullname;
            this.Password = password;
            this.Email = email;
            this.Major = major;
            this.StudentModelId = id;
            
            this.enrollmentStatus = false;
        }
        #endregion CTOR



        public string Password
        {
            get; set;
        }

        public string FullName
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }

        public string Major { get; set; }

        
        public int StudentModelId { get; set; }


        public bool enrollmentStatus { get; set; }

        public void AddCourses(Dictionary<string, CourseModel> s)
        {
            if (schedule.Count + s.Count <= Global.maxCourses)
            {
                foreach (var item in schedule)
                {
                    AddCourse(item.Value);
                }
            }
            else
            {
                throw new IndexOutOfRangeException(Errors.notEnoughSpace);
            }
        }

        public void AddCourse(CourseModel course)
        {
            if (course.isFull == false)
            {
                DataConnection.RegisterStudentForCourse(course.CourseModelId, StudentModelId);
                schedule.Add(course.CourseModelId, course);
            }
            else
            {
                throw new IndexOutOfRangeException(Errors.notEnoughSpace);
            }
        }

        public Dictionary<string, CourseModel> GetSchedule()
        {
            return schedule;
        }

        public override string ToString()
        {
            string result = "";

            result += FullName;
            result += "\n";
            result += $"email: {Email}";

            return result;
        }


    }
}