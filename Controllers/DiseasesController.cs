using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIdemo.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APIdemo.DTOs;
using APIdemo.Tools;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace APIdemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController,Authorize]
    public class DiseasesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DiseasesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Diseases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiseaseOverViewDto>>> GetDiseases()
        {
            return await _context.Diseases
                .Select(d => new DiseaseOverViewDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Image = d.Image
                }).ToListAsync();
        }

        // GET: api/Diseases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiseaseDto>> GetDisease(int id)
        {
            var disease = await _context.Diseases
                .Select(d => new DiseaseDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Specialty = d.Specialty,
                    Symptoms = d.Symptoms,
                    Types = d.Types,
                    Causes = d.Causes,
                    DiagnosticMethods = d.DiagnosticMethods,
                    Prevention = d.Prevention,
                    Image = d.Image
                })
                .FirstOrDefaultAsync(d => d.Id == id);

            if (disease == null)
            {
                return NotFound();
            }

            return disease;
        }
        // GET: api/Diseases/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> SearchDiseases([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }

            var diseases = await _context.Diseases
                .Where(d => d.Title.Contains(name))
                .Select(d => new DiseaseDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Specialty = d.Specialty,
                    Symptoms = d.Symptoms,
                    Types = d.Types,
                    Causes = d.Causes,
                    DiagnosticMethods = d.DiagnosticMethods,
                    Prevention = d.Prevention,
                    Image = d.Image
                }).ToListAsync();

            if (diseases.Count == 0)
            {
                return NotFound($"No diseases found with name containing '{name}'.");
            }

            return Ok(diseases);
        }

        // PUT: api/Diseases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisease(int id, [FromForm] DiseaseDto diseaseDto, [FromForm] IFormFile? imageFile)
        {
            if (id != diseaseDto.Id)
            {
                return BadRequest();
            }

            var disease = await _context.Diseases.FindAsync(id);
            if (disease == null)
            {
                return NotFound();
            }

            disease.Title = diseaseDto.Title;
            disease.Specialty = diseaseDto.Specialty;
            disease.Symptoms = diseaseDto.Symptoms;
            disease.Types = diseaseDto.Types;
            disease.Causes = diseaseDto.Causes;
            disease.DiagnosticMethods = diseaseDto.DiagnosticMethods;
            disease.Prevention = diseaseDto.Prevention;

            if (imageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    disease.Image = memoryStream.ToArray();
                }
                if (disease.Image != null)
                {
                    string? type = ImageTools.GetImageType(disease.Image);
                    if (type == null)
                    {
                        return BadRequest(new {status=false, Message = "unsupported picture type" });
                    }
                }
            }

            _context.Entry(disease).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiseaseExists(id))
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

        // POST: api/Diseases
        [HttpPost]
        public async Task<ActionResult<DiseaseDto>> PostDisease([FromForm] DiseaseDto diseaseDto, [FromForm] IFormFile? imageFile)
        {
            var disease = new Disease
            {
                UserId=User.FindFirstValue(ClaimTypes.NameIdentifier),
                Title = diseaseDto.Title,
                Specialty = diseaseDto.Specialty,
                Symptoms = diseaseDto.Symptoms,
                Types = diseaseDto.Types,
                Causes = diseaseDto.Causes,
                DiagnosticMethods = diseaseDto.DiagnosticMethods,
                Prevention = diseaseDto.Prevention
            };

            if (imageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    disease.Image = memoryStream.ToArray();
                }
                if (disease.Image != null)
                {
                    string? type = ImageTools.GetImageType(disease.Image);
                    if (type == null)
                    {
                        return BadRequest(new { status = false, Message = "unsupported picture type" });
                    }
                }
            }

            _context.Diseases.Add(disease);
            await _context.SaveChangesAsync();

            diseaseDto.Id = disease.Id;
            diseaseDto.Image= disease.Image;
            return CreatedAtAction(nameof(GetDisease), new { id = disease.Id }, diseaseDto);
        }

        // DELETE: api/Diseases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            var disease = await _context.Diseases.FindAsync(id);
            if (disease == null)
            {
                return NotFound();
            }

            _context.Diseases.Remove(disease);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiseaseExists(int id)
        {
            return _context.Diseases.Any(e => e.Id == id);
        }
    }
}
