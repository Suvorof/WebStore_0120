﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //throw new ApplicationException("Ошибочка вышла...");
            //return Content("Hello from controller");
            //return new EmptyResult();
            //return new FileContentResult();
            //return new NotFoundResult();
            //return new JsonResult("");
            //return PartialView("Cart");
            //return RedirectToAction("Blog");
            //return Redirect("https://google.com");
            //return StatusCode(500);
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult _404()
        {
            return View();
        }

        public IActionResult BlogSingle()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ProductDetails()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }
    }
}