using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.DomainNew.Filters;
using WebStore.Infrastructure.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly IProductService _productData;
        public HomeController(IProductService productData)
        {
            _productData = productData;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductList()
        {
            return View(_productData.GetProducts(new ProductFilter()));
        }

        public IActionResult Delete(int id)
        {
            _productData.Delete(id);
            return RedirectToAction(nameof(ProductList));
        }

    }
}
