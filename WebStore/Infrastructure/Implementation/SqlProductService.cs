using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL;
using WebStore.DomainNew.Entities;
using WebStore.DomainNew.Filters;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Implementation
{
    public class SqlProductService : IProductService
    {
        private readonly WebStoreContext _context;

        public SqlProductService(WebStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter)
        {
            var query = _context.Products
                .Include(p => p.Category) // жадная загрузка (Eager Load) для категорий
                .Include(p => p.Brand) // жадная загрузка (Eager Load) для брендов 
                .AsQueryable();
            if (filter.BrandId.HasValue)
                query = query.Where(c => c.BrandId.HasValue && c.BrandId.Value.Equals(filter.BrandId.Value));
            if (filter.CategoryId.HasValue)
                query = query.Where(c => c.CategoryId.Equals(filter.CategoryId.Value));
            return query.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category) // жадная загрузка (Eager Load) для категорий
                .Include(p => p.Brand) // жадная загрузка (Eager Load) для брендов 
                .FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Удалить товар с конкретным id
        /// </summary>
        /// <param name="id"></param>
       public void Delete(int id)
        {
            var product = GetProductById(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
