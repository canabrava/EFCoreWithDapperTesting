using System;

namespace EFCoreWithDapper.Domain.Entities
{
    public class PriceHistoryEntity
    {
        public long Id { get; set; }
        public virtual ProductEntity Product { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
