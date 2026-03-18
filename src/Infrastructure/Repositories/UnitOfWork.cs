using Core.Application.Interfaces;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IGenericRepository<Product> _products;
        private IGenericRepository<Category> _categories;
        private IGenericRepository<Order> _orders;
        private IGenericRepository<User> _users;
        private IGenericRepository<ProductOptionGroup> _productOptionGroups;
        private IGenericRepository<ProductOptionValue> _productOptionValues;
        private IGenericRepository<Voucher> _vouchers;
        private IGenericRepository<UserClaim> _userClaims;
        private IGenericRepository<ComboDetail> _comboDetails;
        private IGenericRepository<AuditLog> _auditLogs;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Product> Products => _products ??= new GenericRepository<Product>(_context);
        public IGenericRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_context);
        public IGenericRepository<Order> Orders => _orders ??= new GenericRepository<Order>(_context);
        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);
        public IGenericRepository<ProductOptionGroup> ProductOptionGroups => _productOptionGroups ??= new GenericRepository<ProductOptionGroup>(_context);
        public IGenericRepository<ProductOptionValue> ProductOptionValues => _productOptionValues ??= new GenericRepository<ProductOptionValue>(_context);
        public IGenericRepository<Voucher> Vouchers => _vouchers ??= new GenericRepository<Voucher>(_context);
        public IGenericRepository<UserClaim> UserClaims => _userClaims ??= new GenericRepository<UserClaim>(_context);
        public IGenericRepository<ComboDetail> ComboDetails => _comboDetails ??= new GenericRepository<ComboDetail>(_context);
        public IGenericRepository<AuditLog> AuditLogs => _auditLogs ??= new GenericRepository<AuditLog>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
