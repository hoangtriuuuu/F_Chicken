using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users/{userId}/claims")]
    [Authorize(Policy = "Admin")]
    public class UserClaimsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserClaimsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetClaims(int userId)
        {
            var claims = await _unitOfWork.UserClaims.GetAsync(uc => uc.UserId == userId);
            return Ok(claims);
        }

        [HttpPost]
        public async Task<IActionResult> AddClaim(int userId, [FromBody] UserClaim claimDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var existingClaim = (await _unitOfWork.UserClaims.GetAsync(uc => 
                uc.UserId == userId && 
                uc.ClaimType == claimDto.ClaimType && 
                uc.ClaimValue == claimDto.ClaimValue)).FirstOrDefault();

            if (existingClaim != null)
                return BadRequest("Claim already exists");

            var claim = new UserClaim
            {
                UserId = userId,
                ClaimType = claimDto.ClaimType,
                ClaimValue = claimDto.ClaimValue
            };

            await _unitOfWork.UserClaims.AddAsync(claim);
            await _unitOfWork.CompleteAsync();

            return Ok(claim);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveClaim(int userId, [FromQuery] string type, [FromQuery] string value)
        {
            var claims = await _unitOfWork.UserClaims.GetAsync(uc => 
                uc.UserId == userId && 
                uc.ClaimType == type && 
                uc.ClaimValue == value);
            
            var claim = claims.FirstOrDefault();

            if (claim == null) return NotFound("Claim not found");

            await _unitOfWork.UserClaims.DeleteAsync(claim);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Claim removed" });
        }
    }
}
