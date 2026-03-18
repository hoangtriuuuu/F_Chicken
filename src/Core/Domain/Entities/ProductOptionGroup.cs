using Core.Domain.Common;
using System.Collections.Generic;

namespace Core.Domain.Entities
{
    public class ProductOptionGroup : BaseEntity
    {
        public int ProductId { get; set; }
        public string GroupName { get; set; }
        public bool IsRequired { get; set; } = false;
        public bool AllowMultiple { get; set; } = false;

        // Navigation
        public Product Product { get; set; }
        public ICollection<ProductOptionValue> OptionValues { get; set; } = new List<ProductOptionValue>();
    }
}
