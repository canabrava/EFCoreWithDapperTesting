using System;

namespace EFCoreWithDapper.Models.Request
{
    public class CreateProductModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceChangedOn { get; set; }
    }
}
