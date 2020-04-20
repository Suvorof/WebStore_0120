using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore
{
    public class EmployeeController : Controller
    {
        private readonly List<EmployeeView> _employees = new List<EmployeeView>
        {
            new EmployeeView
            {
                Id = 1,
                FirstName = "Иван",
                SurName = "Иванов",
                Patronymic = "Иванович",
                Age = 22,
                Position = "Academic"
            },
            new EmployeeView
            {
                Id = 2,
                FirstName = "Antony",
                SurName = "Egorov",
                Patronymic = "Eduardovich",
                Age = 32,
                Position = "WatchMaker"
            }
        };

        // GET:
        // GET: /home/
        // GET: /home/index
        public IActionResult Index()
        {
            return View(_employees);
            //return Content("Hello from controller");
        }

        // GET: /home/details/{id}
        // GET: /home/details/1
        // GET: /home/details/2
        public IActionResult Details(int id)
        {
            // Нам нужен тот сотрудник,id которого совпадает с тем id, который мы передали
            var employee = _employees.FirstOrDefault(x => x.Id == id);
            //Если такого не существует
            if (employee == null)
                return NotFound(); //возвращаем результат 404 Not Found
            return View(employee);
        }

    }
}
