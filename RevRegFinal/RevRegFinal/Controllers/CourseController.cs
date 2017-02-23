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

        public ActionResult AddCourse(int studentModelId, string InputEmail, string InputPassword, string Course)
        {
            string inpute = InputEmail;
            string inputp = InputPassword;
            StudentModel student = DataAccess.getStudent(studentModelId);
            CourseModel course = DataAccess.getCourse(Course);

            if(student.GetSchedule(studentModelId).Count < 6)
            student.AddCourse(course);



            return RedirectToAction("Login", "Main",
                    new { InputEmail = inpute, InputPassword = inputp});
        }
    }
}