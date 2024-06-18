using APIdemo.Models;
using APIdemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIdemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public SearchController(IAuthService authService, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _authService = authService;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("{term}")]
        public async Task<ActionResult<object>> Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return BadRequest("Empty String not allowed");
            }

            
            var diseases = await _context.Diseases
               .Where(d => d.Title.Contains(term))
               .ToListAsync();

            var diseasesDto = diseases.Select(d => new
            {
                Id = d.Id,
                Title = d.Title,
                Specialty = d.Specialty,
                Symptoms = d.Symptoms.Split(","),
                Types = d.Types?.Split(','),
                Causes = d.Causes?.Split(','),
                DiagnosticMethods = d.DiagnosticMethods?.Split(','),
                Prevention = d.Prevention?.Split(','),
                Image = d.Image
            });

            
            var doctors = await _context.Users
                .Where(u => u.DoctorInfo != null && (u.FirstName.Contains(term) || u.LastName.Contains(term)))
                .Include(u => u.DoctorInfo)
                .Select(d => new
                {
                    d.ProfilePicture,
                    UserId = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.DoctorInfo.Specialization??"null"
                })
                .ToListAsync();

            
            var combinedResults = new
            {
                Diseases = diseases,
                Doctors = doctors
            };

            // Return results (even if one is empty)
            return Ok(combinedResults);
        }
    }
}
