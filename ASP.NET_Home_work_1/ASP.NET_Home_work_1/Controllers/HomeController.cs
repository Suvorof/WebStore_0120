using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ASP.NET_Home_work_1.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_Home_work_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<WatchView> _watches = new List<WatchView>
        {
            new WatchView
            {
                Id = 1,
                LabelName = "Rolex",
                BeatPerHour = 28000,
                CaseMaterial = "Gold",
                Sex = "Male",
                NumberOfHands = 3,
                Price = "18000 $",
                TypeOfMechanism = "Mechanical",
                WaterResist = "5 Атм"
            },
            new WatchView
            {
                Id = 2,
                LabelName = "Frederique Constant",
                BeatPerHour = 28000,
                CaseMaterial = "Steel",
                Sex = "Female",
                NumberOfHands = 6,
                Price = "2300 $",
                TypeOfMechanism = "Mechanical",
                WaterResist = "10 Атм"
            }
        };
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_watches);
        }

        public IActionResult Details(int id)
        {
            var watches = _watches.FirstOrDefault(x => x.Id == id);
            if (watches == null)
                return NotFound();
            return View(watches);
        }
    }
}
