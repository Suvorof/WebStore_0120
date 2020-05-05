using WebStore.DomainNew.Entities.Base.Interfaces;

namespace WebStore.Models
{
    public class BrandViewModel : INamedEntity, IOrderedEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Order { get; set; }

        // Количество товаров бренда
        public int ProductCount { get; set; }
    }
}