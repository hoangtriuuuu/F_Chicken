using System;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<ProductOptionGroup> ProductOptionGroups { get; }
        IGenericRepository<ProductOptionValue> ProductOptionValues { get; }
        IGenericRepository<Voucher> Vouchers { get; }
        IGenericRepository<UserClaim> UserClaims { get; }
        IGenericRepository<ComboDetail> ComboDetails { get; }
        IGenericRepository<AuditLog> AuditLogs { get; }
        
        Task<int> CompleteAsync();
    }
}
