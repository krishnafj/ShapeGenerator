using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShapeGenerator.Models;
using ShapeGenerator.Core;

namespace ShapeGenerator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateShape(string UserRequest)
        {
            IShapeValidator validator = new ShapeValidatorService();
            var model = validator.ValidateData(UserRequest);
                
            return Json(model);
        }
    }
}
