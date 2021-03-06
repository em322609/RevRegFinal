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


            //Login will verify whether or not the student exists within the database and then redirect
            //to the student portal
            public ActionResult Login(string InputEmail, string InputPassword)
            {
                    StudentModel student = new StudentModel();
            
           if (DataAccess.verifyLogin(student, InputEmail, InputPassword))
                    {
                        ViewData["studentName"] = student.FullName;
                        ViewData["studentModelId"] = student.StudentModelId;
                        ViewData["studentEmail"] = student.Email;
                        ViewData["studentMajor"] = student.Major;
                        ViewData["enrollmentStatus"] = student.enrollmentStatus;
                        ViewData["inputEmail"] = InputEmail;
                        ViewData["inputPassword"] = InputPassword;

                         List<CourseModel> s = student.GetSchedule(student.StudentModelId);
                         ViewData["listCount"] = s.Count;
                         ViewBag.list = s;

                         return View(student);
                    }
            else {
                return RedirectToAction("Index",
                    new { r = Request.Url.ToString() });
            }

            
            }
        }
}