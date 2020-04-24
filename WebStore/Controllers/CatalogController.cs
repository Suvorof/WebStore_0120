using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        [SimpleActionFilter] // может быть применён как на весь контроллер, так и на методы / метод
        public IActionResult Shop()
        {
            return View();
        }
        public IActionResult ProductDetails()
        {
            return View();
        }

    }
}