using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.Portal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            //HttpContext.Session.SetString("ClientGUId", "ClientGUID-Value");
            return View();
        }


		
       
    }

}
