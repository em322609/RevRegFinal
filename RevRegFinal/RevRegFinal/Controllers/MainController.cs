using System;
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

                    if (DataConnection.CheckLogInInfo(InputEmail, InputPassword))
                    {
 
                        return View();
                    }
                
                return Index();
            }
        }
}