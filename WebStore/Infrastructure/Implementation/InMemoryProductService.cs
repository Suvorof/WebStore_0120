using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Implementation
{
    public class InMemoryProductService : IProductService
    {
        private readonly List<Category> _categories;
        private readonly List<Brand> _brands;

        public InMemoryProductService()
        {
            _categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Sportswear",
                    Order = 0,
                    ParentId = null
                },

                new Category()
                {
                    Id = 2,
                    Name = "Nike",
                    Order = 0,
                    ParentId = 1
                },

                new Category()
                {
                    Id = 3,
                    Name = "Under Armour",
                    Order = 1,
                    ParentId = 1
                },

                new Category()
                {
                    Id = 4,
                    Name = "Adidas",
                    Order = 2,
                    ParentId = 1
                },

                new Category()
                {
                    Id = 5,
                    Name = "Puma",
                    Order = 3,
                    ParentId = 1
                },

                new Category()
                {
                    Id = 6,
                    Name = "Asics",
                    Order = 4,
                    ParentId = 1
                }
            };

            _brands = new List<Brand>()
            {
                new Brand()
                {
                    Id = 1,
                    Name = "Acne",
                    Order = 0
                },

                new Brand()
                {
                    Id = 2,
                    Name = "Grüne Erde",
                    Order = 1
                },

                new Brand()
                {
                    Id = 3,
                    Name = "Albiro",
                    Order = 2
                },

                new Brand()
                {
                    Id = 4,
                    Name = "Ronhill",
                    Order = 3
                },

                new Brand()
                {
                    Id = 5,
                    Name = "Oddmolly",
                    Order = 4
                },

                new Brand()
                {
                    Id = 6,
                    Name = "Boudestijn",
                    Order = 5
                },

                new Brand()
                {
                    Id = 7,
                    Name = "Rösch creative culture",
                    Order = 6
                }
            };

        }
        public IEnumerable<Category> GetCategories()
        {
            return _categories;
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _brands;
        }
    }
}
