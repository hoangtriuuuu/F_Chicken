using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    // [Authorize(Roles = "Manager,Admin")] // Uncomment when auth is fully integrated
    public class AuditLogsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditLogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _unitOfWork.AuditLogs.GetAllAsync();
            return Ok(logs.OrderByDescending(l => l.CreatedAt));
        }

        [HttpGet("entity/{entityName}/{entityId}")]
        public async Task<IActionResult> GetByEntity(string entityName, int entityId)
        {
            var logs = await _unitOfWork.AuditLogs.GetAsync(l => 
                l.EntityName.ToLower() == entityName.ToLower() && l.EntityId == entityId);
            return Ok(logs.OrderByDescending(l => l.CreatedAt));
        }
    }
}
