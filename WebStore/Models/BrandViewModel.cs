using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Models
{
    public class BrandViewModel : INamedEntity, IOrderedEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Order { get; set; }
        public int ProductCount { get; set; }
    }
}