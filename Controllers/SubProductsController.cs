using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArawanMarbleApi.Models;
using ArawanMarbleApi.Dto;
using Microsoft.AspNetCore.Authorization;

namespace ArawanMarbleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubProductsController : ControllerBase
    {
        private readonly Ara56nmarblecomContext _context;

        public SubProductsController(Ara56nmarblecomContext context)
        {
            _context = context;
        }
        [HttpGet("byProduct/{productId}")]
        public async Task<ActionResult<IEnumerable<SubProduct>>> GetSubProductsByProductId(int productId)
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
            return await _context.SubProducts
                .Where(sp => sp.Productid == productId)
                .ToListAsync();
        }
        // GET: api/SubProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubProduct>>> GetSubProducts()
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
            return await _context.SubProducts.ToListAsync();
        }

        // GET: api/SubProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubProduct>> GetSubProduct(int id)
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
            var subProduct = await _context.SubProducts.FindAsync(id);

            if (subProduct == null)
            {
                return NotFound();
            }

            return subProduct;
        }

        // PUT: api/SubProducts/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubProduct(int id, SubProduct subProduct)
        {
            if (id != subProduct.SubProductid)
            {
                return BadRequest();
            }

            _context.Entry(subProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SubProducts
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostSubProduct([FromForm] SubProductUploadDto dto)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Productimg.FileName)}";
            var filePath = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Productimg.CopyToAsync(stream);
            }

            var subProduct = new SubProduct
            {
                Productid = dto.Productid,
                Productimg = "/images/" + fileName,
                Productname = "q"
            };

            _context.SubProducts.Add(subProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubProduct", new { id = subProduct.SubProductid }, subProduct);
        }



        // DELETE: api/SubProducts/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubProduct(int id)
        {
            var subProduct = await _context.SubProducts.FindAsync(id);
            if (subProduct == null)
            {
                return NotFound();
            }

            _context.SubProducts.Remove(subProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize]
        private bool SubProductExists(int id)
        {
            return _context.SubProducts.Any(e => e.SubProductid == id);
        }
    }
}
