using APIdemo.DTOs;
using APIdemo.Models;
using APIdemo.Services;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace APIdemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public DoctorsController(IAuthService authService, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _authService = authService;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("search-all")]
        public async Task<ActionResult<object>> Search([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }

            // Search for Diseases
            var diseases = await _context.Diseases
               .Where(d => d.Title.Contains(name))
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

            // Search for Doctors
            var doctors = await _context.Users
                .Where(u => u.DoctorInfo != null && (u.FirstName.Contains(name) || u.LastName.Contains(name)))
                .Include(u => u.DoctorInfo)
                .Select(d => new 
                {
                    UserId = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.DoctorInfo.Specialization??"null"
                })
                .ToListAsync();

            // Combine Results
            var combinedResults = new
            {
                Diseases = diseases,
                Doctors = doctors
            };

            // Return results (even if one is empty)
            return Ok(combinedResults);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            // Retrieve all doctors from the database
            var doctors = await _context.Users.Where(u=>u.DoctorInfo != null)
                .Include(u=>u.DoctorInfo)
                .ThenInclude(u=>u.WorkingDays)
                .ToListAsync();

            // Map to DTOs
            var doctorDTOs = doctors.Select(d => new DoctorDto
            {
                UserId = d.Id,
                FirstName= d.FirstName,
                LastName= d.LastName,
                Specialization = d.DoctorInfo.Specialization
            }).ToList();

            return Ok(doctorDTOs);
        }
        // GET: api/Doctors/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> SearchDiseases([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }

            var doctors = await _context.Users.Where(u => u.DoctorInfo != null&&(u.FirstName.Contains(name)||u.LastName.Contains(name)))
                .Include(u => u.DoctorInfo)
                .ThenInclude(u => u.WorkingDays)
                .ToListAsync();

            if (doctors.Count == 0)
            {
                return NotFound($"No doctor found with name containing '{name}'.");
            }
            var doctorDTOs = doctors.Select(d => new DoctorDto
            {
                UserId = d.Id,
                FirstName= d.FirstName,
                LastName= d.LastName,
                Specialization = d.DoctorInfo.Specialization
            }).ToList();

            return Ok(doctors);
        }
        [HttpPost("register")]
        public async Task<IActionResult> DoctorRegisterAsync([FromForm] DoctorRegisterDto model, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model, lang,"Doctor");

            if (!result.IsAuthenticated)
            {
                return BadRequest(new { result.Message, result.Errors, status = false });
            }
           

            var doctorInfo = new DoctorInfo
            {
                UserId = (await _userManager.FindByEmailAsync(model.Email)).Id,
                Experience = model.Experience,
                ClinicFees = model.ClinicFees,
                Description = model.Description,
                Services = string.Join(",", model.Services),
                Specialization = model.Specialization
            };

            await _context.DoctorInfos.AddAsync(doctorInfo);
            await _context.SaveChangesAsync();
            foreach (var day in model.WorkingDays)
            {
                await _context.DoctorWorkingDays.AddAsync(new DoctorWorkingDay { DoctorInfoId = doctorInfo.Id, Day = day.Day ,OpenAt=day.OpenAt,CloseAt=day.CloseAt});
            }

            await _context.SaveChangesAsync();

            return Ok(result);
        }
        [HttpPut("update-profile"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateProfile(DoctorRegisterDto profile, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new { status = false, message = lang == "ar" ? "لم يتم العثور علي مستخدم" : "User not found" });
            }

            // Update ApplicationUser properties
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.BirthDate = profile.BirthDate;
            user.PhoneNumber = profile.PhoneNumber;
            user.Governorate = profile.Governorate;
            user.Latitude = profile.Latitude;
            user.Longitude = profile.Longitude;
            user.Address = profile.Address;

            if (profile.Email.ToUpper() != user.NormalizedEmail)
            {
                if ((await _userManager.FindByEmailAsync(profile.Email)) != null)
                {
                    return BadRequest(new { status = false, message = lang == "ar" ? "الحساب مستخدم بالفعل" : "Email is already used" });
                }
                user.Email = profile.Email;
                user.UserName = profile.Email;
                user.EmailConfirmed = false;
            }

            var userUpdateResult = await _userManager.UpdateAsync(user);
            if (!userUpdateResult.Succeeded)
            {
                return BadRequest(new { status = false, message = string.Join(", ", userUpdateResult.Errors.Select(e => e.Description)) });
            }

            // Update DoctorInfo properties
            var doctorInfo = await _context.DoctorInfos.Include(d => d.WorkingDays).FirstOrDefaultAsync(d => d.UserId == user.Id);
            if (doctorInfo == null)
            {
                return NotFound(new { status = false, message = lang == "ar" ? "لم يتم العثور علي الطبيب" : "Doctor not found" });
            }

            doctorInfo.Experience = profile.Experience;
            doctorInfo.Description = profile.Description;
            doctorInfo.ClinicFees = profile.ClinicFees;
            doctorInfo.Services = string.Join(",", profile.Services);
            doctorInfo.Specialization = profile.Specialization;

            _context.DoctorInfos.Update(doctorInfo);

            _context.DoctorWorkingDays.RemoveRange(doctorInfo.WorkingDays);
            foreach (var day in profile.WorkingDays)
            {
                _context.DoctorWorkingDays.Add(new DoctorWorkingDay { DoctorInfoId = doctorInfo.Id, Day = day.Day, OpenAt=day.OpenAt, CloseAt=day.CloseAt });
            }

            await _context.SaveChangesAsync();

            return Ok(new { status = true, message = lang == "ar" ? "تم تحديث الملف الشخصي بنجاح" : "Profile updated successfully" });
        }
        [HttpGet("nearby-doctors")]
        [Authorize]
        public async Task<IActionResult> GetNearbyDoctors([FromHeader] string? lang = "en", double radius = 10.0)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new { message = lang == "ar" ? "لم يتم العثور علي مستخدم" : "User not found" });
            }

            if (user.Latitude == null || user.Longitude == null)
            {
                return BadRequest(new { message = lang == "ar" ? "موقع المستخدم غير متاح" : "User location not available" });
            }

            var userLocation = new GeoCoordinate((double)user.Latitude.Value,(double)user.Longitude.Value);
            var doctors = await _context.DoctorInfos
                                        .Include(d => d.User)
                                        .Include(d => d.WorkingDays)
                                        .ToListAsync();

            var today = DateTime.UtcNow.DayOfWeek;

            var nearbyDoctors = doctors.Where(d =>
            {
                if (d.User.Latitude == null || d.User.Longitude == null)
                    return false;

                var doctorLocation = new GeoCoordinate(d.User.Latitude.Value, d.User.Longitude.Value);
                return userLocation.GetDistanceTo(doctorLocation) <= radius * 1000;
            }).Select(d => new
            {
                d.UserId,
                d.User.FirstName,
                d.User.LastName,
                d.Description,
                d.User.ProfilePicture
            }).ToList();

            return Ok(new
            {
                message = lang == "ar" ? "تم العثور على الأطباء القريبين" : "Nearby doctors found",
                doctors = nearbyDoctors
            });
        }

    }
}
