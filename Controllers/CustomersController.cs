using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArawanMarbleApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace ArawanMarbleApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly Ara56nmarblecomContext _context;

        public CustomersController(Ara56nmarblecomContext context)
        {
            _context = context;
        }
        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Customerid)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            // Kullanıcının session'ında form gönderim zamanı var mı kontrol et
            var lastSubmission = HttpContext.Session.GetString("LastFormSubmission");

            // Eğer session'da zaman yoksa veya 1 dakikadan fazla geçmişse
            if (lastSubmission == null || DateTime.Parse(lastSubmission) < DateTime.Now.AddMinutes(-10))
            {
                // Formu veritabanına ekle
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                // Şu anki zaman damgasını session'a kaydedin
                HttpContext.Session.SetString("LastFormSubmission", DateTime.Now.ToString());

                return CreatedAtAction("GetCustomer", new { id = customer.Customerid }, customer);
            }

            // Eğer 1 dakika içerisinde form tekrar gönderilmişse
            return BadRequest("Çok fazla istek gönderildi. Lütfen daha sonra tekrar deneyin.");
        }

        
        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Customerid == id);
        }
    }
}
