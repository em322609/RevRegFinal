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

        public ActionResult AddCourse(string InputEmail, string InputPassword, CourseModel Course)
        {
            string inpute = InputEmail;
            string inputp = InputPassword;


            return RedirectToAction("Login", "Main",
                    new { InputEmail = inpute, InputPassword = inputp});
        }
    }
}