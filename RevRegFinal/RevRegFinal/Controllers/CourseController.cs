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

            List<CourseModel> list = DataConnection.getCourses();
            ViewBag.list = list;
            ViewData["inputEmail"] = InputEmail;
            ViewData["inputPassword"] = InputPassword;
            ViewData["studentModelId"] = studentModelId;
            //ViewBag.student = DataConnection.getStudent();
            return View();
        }

        public ActionResult AddCourse(int studentModelId, string InputEmail, string InputPassword, string Course)
        {
            string inpute = InputEmail;
            string inputp = InputPassword;
            StudentModel student = DataConnection.getStudent(studentModelId);
            CourseModel course = DataConnection.getCourse(Course);
            student.AddCourse(course);



            return RedirectToAction("Login", "Main",
                    new { InputEmail = inpute, InputPassword = inputp});
        }
    }
}