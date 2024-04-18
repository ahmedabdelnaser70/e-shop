using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var Products = await _context.Products.ToListAsync();
            return Products;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductID(int id)
        {
            var Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            return Product;
        }

    }
}
