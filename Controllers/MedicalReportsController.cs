using SkinCaApp.DTOs;
using SkinCaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SkinCaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class MedicalReportsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public MedicalReportsController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/MedicalReport
        [HttpGet]
        public async Task<IActionResult> GetMedicalReports()
        {
            var user = await _userManager.GetUserAsync(User);
            var list =  await _context.MedicalReports
                .Where(m => m.UserId == user.Id)
                .Select(m => new { 
                    m.Id,
                    m.ReportName,
                    m.Content,
                    m.ReportType,
                    m.Report
                }) 
                .ToListAsync();
            return Ok(list);
        }

        // GET: api/MedicalReport/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalReport(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var medicalReport = await _context.MedicalReports.FindAsync(id);
            if (medicalReport == null || medicalReport.UserId != user.Id)
            {
                return NotFound();
            }

            return Ok(new MedicalReportDto(medicalReport));
        }

        // POST: api/MedicalReport
        [HttpPost]
        public async Task<ActionResult<MedicalReportDto>> PostMedicalReport([FromForm] MedicalReportCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Enforce Id requirement
            }
            var user = await _userManager.GetUserAsync(User);
            var medicalReport = new MedicalReport
            {
                UserId = user.Id,
                ReportName = model.ReportName,
                Content = model.Content,
                ReportType = model.ReportType
            };

            // Handle image upload
            if (model.ReportFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ReportFile.CopyToAsync(memoryStream);
                    medicalReport.Report = memoryStream.ToArray();
                }
            }

            _context.MedicalReports.Add(medicalReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalReport", new { id = medicalReport.Id }, new MedicalReportDto(medicalReport));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<MedicalReportDto>> PutMedicalReport(int id,[FromForm] MedicalReportCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.GetUserAsync(User);
            var existingReport = await _context.MedicalReports.FindAsync(id);
            if (existingReport == null)
            {
                return NotFound();
            }
            existingReport.ReportName = model.ReportName;
            existingReport.ReportType= model.ReportType;
            existingReport.Content=model.Content;

            
            if (model.ReportFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ReportFile.CopyToAsync(memoryStream);
                    existingReport.Report = memoryStream.ToArray();
                }
            }

            _context.MedicalReports.Update(existingReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalReport", new { id = existingReport.Id }, new MedicalReportDto(existingReport));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = await _context.MedicalReports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.MedicalReports.Remove(report);
            await _context.SaveChangesAsync();

            return Ok(report);
        }
    }
}
