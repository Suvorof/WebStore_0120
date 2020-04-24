using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore
{
    //до ~/employee
    [Route("users")]
    //после ~/users
    public class EmployeeController : Controller
    {
        private readonly IEmployeesService _employeesService;

        // Этой строчкой мы внедряем зависимость, т.е используем механизм внедрения зависимостей depandancy injection
        // при этом мы не обращаемся к контроллеру т.е. не создаём экземпляр его класса это за нас делает механизм
        // depandancy injection, который находится под капотом Asp.Net Core и в этом и есть вся красота и элегантность
        public EmployeeController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        // GET:
        // GET: /home/
        // GET: /home/index
        [Route("all")]
        public IActionResult Index()
        {
            return View(_employeesService.GetAll());
            //return Content("Hello from controller");
        }

        // GET: /home/details/{id}
        // GET: /home/details/1
        // GET: /home/details/2
        [Route("{id}")] // id взят в {}, чтобы он прибиндился к id в параметре метода Details
        // ~/users/1111
        public IActionResult Details(int id)
        {
            // Нам нужен тот сотрудник,id которого совпадает с тем id, который мы передали
            var employee = _employeesService.GetById(id);
            //Если такого не существует
            if (employee == null)
                return NotFound(); //возвращаем результат 404 Not Found
            return View(employee);
        }
        [HttpGet]
        [Route("edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return View(new EmployeeView());

            EmployeeView model = _employeesService.GetById(id.Value);
            if (model == null)
                return NotFound(); // возвращаем результат 404 Not Found

            return View(model);
        }

        [HttpPost]
        [Route("edit/{id?}")]
        public IActionResult Edit(EmployeeView model)
        {
            if (model.Age < 18 || model.Age > 100)
            {
                ModelState.AddModelError("Age", "Ошибка возраста");
            }

            // Если хоть одно поле в модели неправильно заполнено (невалидно), то мы вернём модель заново
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Id > 0) // если есть Id, то редактируем модель
            {
                var dbItem = _employeesService.GetById(model.Id);

                if (ReferenceEquals(dbItem, null))
                    return NotFound(); //возвращаем результат 404 Not Found

                dbItem.SurName = model.SurName;
                dbItem.FirstName = model.FirstName;
                dbItem.Age = model.Age;
                dbItem.Patronymic = model.Patronymic;
                dbItem.Patronymic = model.Patronymic;
            }
            else // иначе добавляем модель в список
            {
                _employeesService.AddNew(model);
            }
            _employeesService.Commit(); // станет актуальным позднее (когда добавим БД)
            return RedirectToAction(nameof(Index));
        }

        [Route("delete/{id}")] // id без ? так как его присутствие обязательно.
        public IActionResult Delete(int id)
        {
            _employeesService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
