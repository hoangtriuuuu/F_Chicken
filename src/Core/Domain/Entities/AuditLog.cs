using Core.Domain.Common;
using System;

namespace Core.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; } // ChangePrice, ChangeStatus, etc.
        public string EntityName { get; set; } // Product, Order
        public int EntityId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string? Note { get; set; }
    }
}
