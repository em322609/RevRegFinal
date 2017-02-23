﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RevRegFinal.Models;

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
                    StudentModel student = new StudentModel();
            
           if (DataConnection.CheckLogInInfo(student, InputEmail, InputPassword))
                    {
                        ViewData["studentName"] = student.FullName;
                        ViewData["studentModelId"] = student.StudentModelId;
                        ViewData["inputEmail"] = InputEmail;
                        ViewData["inputPassword"] = InputPassword;

                         Dictionary<string, CourseModel> s = student.GetSchedule();
                         ViewData["dictionaryCount"] = s.Count;

                         return View(student);
                    }
            else {
                return RedirectToAction("Index",
                    new { r = Request.Url.ToString() });
            }

            
            }
        }
}