using System;
using System.ComponentModel.DataAnnotations.Schema;
using CloudNativeKit.Domain;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;
using static CloudNativeKit.Utils.Helpers.DateTimeHelper;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ProductCatalog.Domain
{
    [Table("Products", Schema ="catalog")]
    public class Product : AggregateRootBase<Guid>
    {
        private Product() : base(NewId())
        {
        }

        private Product(Guid id) : base(id)
        {
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public double Price { get; private set; }

        public string ImageUrl { get; private set; }

        public bool IsDeleted { get; private set; }

        public static Product Of(CreateProductRequest request)
        {
            return new Product
            {
                Name = request.Name,
                Description = request.Desc,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                Updated = NewDateTime(),
                IsDeleted = false
            };
        }

        public Product UpdateProduct(UpdateProductRequest request)
        {
            Name = request.Name;
            Description = request.Desc;
            Price = request.Price;
            ImageUrl = request.ImageUrl;
            Updated = NewDateTime();

            AddEvent(new ProductUpdated
            {
                Id = Id.ToString(),
                Name = Name,
                Price = Price,
                ImageUrl = ImageUrl,
                Desc = Description
            });

            return this;
        }

        public Product MarkAsDeleted()
        {
            IsDeleted = true;

            AddEvent(new ProductDeleted
            {
                Id = Id.ToString()
            });

            return this;
        }
    }
}