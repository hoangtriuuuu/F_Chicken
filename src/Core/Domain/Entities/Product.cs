using Core.Domain.Common;
using System.Collections.Generic;

namespace Core.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string ImageUrl { get; set; }
        public bool IsCombo { get; set; }
        public bool IsActive { get; set; } = true;
        
        public int CategoryId { get; set; }
        
        // Navigation
        public Category Category { get; set; }
        public ICollection<ProductOptionGroup> OptionGroups { get; set; } = new List<ProductOptionGroup>();
        public ICollection<ComboDetail> ComboItems { get; set; } = new List<ComboDetail>();
    }
}
