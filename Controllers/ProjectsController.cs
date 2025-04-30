using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArawanMarbleApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace ArawanMarbleApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly Ara56nmarblecomContext _context;
        private readonly IWebHostEnvironment _environment;
        public ProjectsController(Ara56nmarblecomContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
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

            return await _context.Projects.ToListAsync();
        }
        // GET: api/Projects/nine
        [HttpGet("nine")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsNine()
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

            return await _context.Projects.Take(9).ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
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

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Projectid)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProject([FromForm] ProjectCreateModel model)
        {
            if (model.ProductImage == null || model.ProductImage.Length == 0)
                return BadRequest("You must upload an image file.");

            // Resim kaydetme işlemi
            var imagesFolder = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProductImage.FileName);
            var filePath = Path.Combine(imagesFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProductImage.CopyToAsync(stream);
            }

            // Veritabanına kayıt
            var project = new Project
            {
                Projectname = model.ProjectName,
                Description = model.Description,
                Projectimg = "/images/" + uniqueFileName // doğru kayıt ✅
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Project successfully added." });
        }


        public class ProjectCreateModel
        {
            public string ProjectName { get; set; }
            public string? Description { get; set; }
            public IFormFile ProductImage { get; set; }
        }

        // DELETE: api/Projects/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(project.Projectimg))
            {
                // /images/ yolunu wwwroot/images/ yolu ile birleştiriyoruz
                var fileName = Path.GetFileName(project.Projectimg); // Dosya adını alıyoruz
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
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Projectid == id);
        }
    }
}
