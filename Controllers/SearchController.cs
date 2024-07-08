using APIdemo.DTOs;
using APIdemo.DTOs.Response;
using APIdemo.Models;
using APIdemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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

            term = term.ToLower();

            // Fetch only the necessary data from the database for diseases
            var diseases = await _context.Diseases
                .Where(d => d.Title.ToLower().Contains(term))
                .Select(d => new
                {
                    d.Id,
                    d.Title,
                    d.Specialty,
                    d.Symptoms,
                    d.Types,
                    d.Causes,
                    d.DiagnosticMethods,
                    d.Prevention,
                    d.Image
                })
                .ToListAsync();

            // Separate and process StartsWith and Contains results
            var diseasesStartsWith = diseases
                .Where(d => d.Title.ToLower().StartsWith(term))
                .Select(d => new DiseaseDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Specialty = d.Specialty,
                    Symptoms = d.Symptoms.Split(","),
                    Types = d.Types?.Split(","),
                    Causes = d.Causes?.Split(","),
                    DiagnosticMethods = d.DiagnosticMethods?.Split(","),
                    Prevention = d.Prevention?.Split(","),
                    Image = d.Image
                }).ToList();

            var diseasesContains = diseases
                .Where(d => !d.Title.ToLower().StartsWith(term))
                .Select(d => new DiseaseDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Specialty = d.Specialty,
                    Symptoms = d.Symptoms.Split(","),
                    Types = d.Types?.Split(","),
                    Causes = d.Causes?.Split(","),
                    DiagnosticMethods = d.DiagnosticMethods?.Split(","),
                    Prevention = d.Prevention?.Split(","),
                    Image = d.Image
                }).ToList();

            // Combine the results
            var diseasesDto = diseasesStartsWith.Concat(diseasesContains).ToList();

            // Fetch only the necessary data from the database for doctors
            var doctors = await _context.Users
                .Where(u => u.DoctorInfo != null && (u.FirstName.ToLower().Contains(term) || u.LastName.ToLower().Contains(term)))
                .Include(u => u.DoctorInfo)
                .ThenInclude(u => u.WorkingDays)
                .Select(u => new
                {
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.Address,
                    u.Latitude,
                    u.Longitude,
                    u.PhoneNumber,
                    u.ProfilePicture,
                    u.Id,
                    u.DoctorInfo.Specialization,
                    u.DoctorInfo.Description,
                    u.DoctorInfo.Services,
                    u.DoctorInfo.ClinicFees,
                    u.DoctorInfo.Experience,
                    WorkingDays = u.DoctorInfo.WorkingDays.Select(wd => new
                    {
                        wd.Day,
                        wd.CloseAt,
                        wd.OpenAt
                    }).ToList()
                })
                .ToListAsync();

            // Separate and process StartsWith and Contains results for doctors
            var doctorsStartsWith = doctors
                .Where(d => d.FirstName.ToLower().StartsWith(term))
                .Select(d => new DoctorDetailResponseDto
                {
                    Email = d.Email,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.Specialization,
                    Description = d.Description,
                    Services = d.Services.Split(","),
                    Address = d.Address,
                    Rating = Math.Round(5 * Random.Shared.Next(90, 100) * 0.01, 2),
                    ClinicFees = d.ClinicFees,
                    Experience = d.Experience,
                    Latitude = d.Latitude,
                    Longitude = d.Longitude,
                    PhoneNumber = d.PhoneNumber,
                    ProfilePicture = d.ProfilePicture,
                    UserId = d.Id,
                    IsWorking = d.WorkingDays.Any(wd => wd.Day == DateTime.Now.DayOfWeek && DateTime.Now.TimeOfDay >= wd.OpenAt && DateTime.Now.TimeOfDay <= wd.CloseAt),
                    WorkingTime = d.WorkingDays.Where(wd => wd.Day == DateTime.Now.DayOfWeek).Select(wd => new WorkingDayResponseDto
                    {
                        Day = wd.Day.ToString(),
                        CloseAt = wd.CloseAt,
                        OpenAt = wd.OpenAt
                    }).FirstOrDefault(),
                    ClinicSchedule = d.WorkingDays.Select(wd => new WorkingDayResponseDto
                    {
                        Day = wd.Day.ToString(),
                        CloseAt = wd.CloseAt,
                        OpenAt = wd.OpenAt
                    }).ToList()
                }).ToList();

            var doctorsContains = doctors
                .Where(d => !d.FirstName.ToLower().StartsWith(term) && (d.FirstName.ToLower().Contains(term) || d.LastName.ToLower().Contains(term)))
                .Select(d => new DoctorDetailResponseDto
                {
                    Email = d.Email,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.Specialization,
                    Description = d.Description,
                    Services = d.Services.Split(","),
                    Address = d.Address,
                    Rating = Math.Round(5 * Random.Shared.Next(90, 100) * 0.01, 2),
                    ClinicFees = d.ClinicFees,
                    Experience = d.Experience,
                    Latitude = d.Latitude,
                    Longitude = d.Longitude,
                    PhoneNumber = d.PhoneNumber,
                    ProfilePicture = d.ProfilePicture,
                    UserId = d.Id,
                    IsWorking = d.WorkingDays.Any(wd => wd.Day == DateTime.Now.DayOfWeek && DateTime.Now.TimeOfDay >= wd.OpenAt && DateTime.Now.TimeOfDay <= wd.CloseAt),
                    WorkingTime = d.WorkingDays.Where(wd => wd.Day == DateTime.Now.DayOfWeek).Select(wd => new WorkingDayResponseDto
                    {
                        Day = wd.Day.ToString(),
                        CloseAt = wd.CloseAt,
                        OpenAt = wd.OpenAt
                    }).FirstOrDefault(),
                    ClinicSchedule = d.WorkingDays.Select(wd => new WorkingDayResponseDto
                    {
                        Day = wd.Day.ToString(),
                        CloseAt = wd.CloseAt,
                        OpenAt = wd.OpenAt
                    }).ToList()
                }).ToList();

            // Combine the results
            var doctorDTOs = doctorsStartsWith.Concat(doctorsContains).ToList();

            return Ok(new
            {
                Diseases = diseasesDto,
                Doctors = doctorDTOs
            });

        }
    }
}
