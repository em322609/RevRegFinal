using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RevRegFinal.Controllers
{
   
        // GET: Main
        public class MainController : Controller
        {
            // GET: Main
            public ActionResult Index()
            {
                    return View();
               
            }


            public ActionResult Login(string InputEmail, string InputPassword)
            {

               


               /* foreach (var item in )
                {
                    if (InputEmail == item.Email && InputPassword == item.Password)
                    {
                        ViewData["studentName"] = item.FullName;
                        ViewData["courseList"] = item.CourseList;

                        return View(item);
                    }
                }
                */
                return Index();
            }
        }
}