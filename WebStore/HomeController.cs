using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore
{
    public class HomeController : Controller
    {
        private readonly List<EmployeeView> _employees = new List<EmployeeView>
        {
            new EmployeeView
            {
                Id = 1,
                FirstName = "Иван",
                SurName = "Иванов",
                Patronymic = "Иванович",
                Age = 22
            },
            new EmployeeView
            {
                Id = 2,
                FirstName = "Antony",
                SurName = "Egorov",
                Patronymic = "Eduardovich",
                Age = 32
            }
        };

        // GET:
        // GET: /home/
        // GET: /home/index
        public IActionResult Index()
        {
            return View(_employees);
            return Content("Hello from controller");
        }
    }
}
