using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArawanMarbleApi.Models;
using System.IO;
namespace ArawanMarbleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ArawanDbContext _context;

        public ProductsController(ArawanDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductUpdateDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Productname = dto.ProductName;
            product.Description = dto.Description;

            if (dto.ProductImage != null)
            {
                var fileName = Path.GetFileName(dto.ProductImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ProductImage.CopyToAsync(stream);
                }

                product.Productimg = "/images/" + fileName;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
        public class ProductUpdateDto
        {
            public string ProductName { get; set; }
            public string Description { get; set; }
            public IFormFile? ProductImage { get; set; }
        }


        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Productid == id);
        }
    }
}
