using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArawanMarbleApi.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
namespace ArawanMarbleApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Ara56nmarblecomContext _context;

        public ProductsController(Ara56nmarblecomContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var referer = Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(referer) || !Uri.TryCreate(referer, UriKind.Absolute, out var uri))
            {
                return NotFound("Page not found.");
            }

            var allowedHosts = new[] { "arawanmarble.com", "www.arawanmarble.com", "localhost" };

            if (!allowedHosts.Contains(uri.Host))
            {
                return NotFound("Page not found.");
            }

            return await _context.Products.ToListAsync();
        }
        [HttpGet("nine")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsNine()
        {
            var referer = Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(referer) || !Uri.TryCreate(referer, UriKind.Absolute, out var uri))
            {
                return NotFound("Page not found.");
            }

            var allowedHosts = new[] { "arawanmarble.com", "www.arawanmarble.com", "localhost" };

            if (!allowedHosts.Contains(uri.Host))
            {
                return NotFound("Page not found.");
            }

            return await _context.Products.Take(9).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var referer = Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(referer) || !Uri.TryCreate(referer, UriKind.Absolute, out var uri))
            {
                return NotFound("Page not found.");
            }

            var allowedHosts = new[] { "arawanmarble.com", "www.arawanmarble.com", "localhost" };

            if (!allowedHosts.Contains(uri.Host))
            {
                return NotFound("Page not found.");
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] ProductUpdateDto dto)
        {
            if (dto.ProductImage != null)
            {
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // "images" klasörü mevcut değilse oluştur
                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ProductImage.FileName);
                var filePath = Path.Combine(imagesFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ProductImage.CopyToAsync(stream);
                }

                var product = new Product
                {
                    Productname = dto.ProductName,
                    Description = dto.Description,
                    Productimg = $"/images/{uniqueFileName}"  // Frontend'de kullanılabilir yol
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Tüm ürünleri döndür
                var productsList = await _context.Products.ToListAsync();

                return Ok(new
                {
                    message = "Product has been saved to the database ✔️",
                    productId = product.Productid,
                    imagePath = product.Productimg,
                    allProducts = productsList
                });
            }

            return BadRequest("Image could not be uploaded ❌");
        }
        [Authorize]
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

        [Authorize]
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(product.Productimg))
            {
                // /images/ yolunu wwwroot/images/ yolu ile birleştiriyoruz
                var fileName = Path.GetFileName(product.Productimg); // Dosya adını alıyoruz
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                // Dosya var mı diye kontrol et
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        // Dosyayı sil
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred while deleting the file: {ex.Message}");
                    }
                }
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize]
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Productid == id);
        }
    }
}
