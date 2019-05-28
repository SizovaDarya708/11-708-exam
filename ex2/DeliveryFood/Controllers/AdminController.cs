using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFood.Controllers
{
    public class AdminController : Controller
    {
        private RUNContext db;
        private IHostingEnvironment env;
        public IActionResult Index()
        {
            return View();
        }
    }
}