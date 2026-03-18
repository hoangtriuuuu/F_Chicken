using Core.Application.DTOs;
using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// MOI: Commented out to prevent conflict with ProductsController.cs
// namespace WebAPI.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ProductController : ControllerBase
//     {
//         private readonly IProductService _productService;
// 
//         public ProductController(IProductService productService)
//         {
//             _productService = productService;
//         }
// 
//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var products = await _productService.GetAllProductsAsync();
//             return Ok(products);
//         }
// 
//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetById(int id)
//         {
//             var product = await _productService.GetProductByIdAsync(id);
//             if (product == null) return NotFound();
//             return Ok(product);
//         }
// 
//         [HttpPost]
//         public async Task<IActionResult> Create(CreateProductDto request)
//         {
//             var product = await _productService.CreateProductAsync(request);
//             return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
//         }
//     }
// }
