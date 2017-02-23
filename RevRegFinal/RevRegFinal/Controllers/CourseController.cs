using RevRegFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RevRegFinal.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Courses(int studentModelId, string InputEmail, string InputPassword)
        {

            List<CourseModel> list = DataAccess.getCourses();
            ViewBag.list = list;
            ViewData["inputEmail"] = InputEmail;
            ViewData["inputPassword"] = InputPassword;
            ViewData["studentModelId"] = studentModelId;
            
            return View();
        }

        //add course will check whether or not the course being clicked is compatible to be added to 
        //the student schedule based on the constraints listed int he requirements.

        public ActionResult AddCourse(int studentModelId, string InputEmail, string InputPassword, string Course)
        {
            string inpute = InputEmail;
            string inputp = InputPassword;
            StudentModel student = DataAccess.getStudent(studentModelId);
            CourseModel course = DataAccess.getCourse(Course);
            List<CourseModel> list = student.GetSchedule(studentModelId);

            foreach (var item in list)
            {
                if(item.timeOfDay == course.timeOfDay)
                {
                    return RedirectToAction("Login", "Main",
                    new { InputEmail = inpute, InputPassword = inputp });
                }
            }
            if (student.GetSchedule(studentModelId).Count < 6)
            student.AddCourse(course);



            return RedirectToAction("Login", "Main",
                    new { InputEmail = inpute, InputPassword = inputp});
        }
    }
}