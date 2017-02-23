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
        public ActionResult Courses()
        {

            List<CourseModel> list = DataConnection.getCourses();
            ViewBag.list = list;
            return View();
        }
    }
}