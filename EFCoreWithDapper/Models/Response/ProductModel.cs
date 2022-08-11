using System;

namespace EFCoreWithDapper.Models.Response
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        public string IdString
        {
            get { return Id.ToString("N"); }
            set { Id = new Guid(value); }
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceChangedOn { get; set; }
    }
}
