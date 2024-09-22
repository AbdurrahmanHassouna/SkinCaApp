using SkinCaApp.DTOs;
using SkinCaApp.Models;
using SkinCaApp.Services;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SkinCaApp.Controllers
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

        [HttpGet("working")]
        public async Task<IActionResult> GetWorkingDoctors()
        {

            var doctors = await _context.Users.Where(u => u.DoctorInfo != null)
                .Include(u => u.DoctorInfo)
                .ThenInclude(u => u.WorkingDays)
                .ToListAsync();
            if (!doctors.Any())
            {
                return BadRequest();
            }
            var doctorDTOs = doctors.Select(d => new DoctorDetailResponseDto
            {
                Email = d.Email,
                FirstName = d.FirstName,
                LastName  = d.LastName,
                Specialization=d.DoctorInfo.Specialization,
                Description = d.DoctorInfo.Description,
                Services=d.DoctorInfo.Services.Split(","),
                Address = d.Address,
                ClinicFees=d.DoctorInfo.ClinicFees,
                Experience=d.DoctorInfo.Experience,
                Latitude=d.Latitude,
                Longitude=d.Longitude,
                PhoneNumber=d.PhoneNumber,
                ProfilePicture = d.ProfilePicture,
                UserId=d.Id,
                IsWorking=(d.DoctorInfo.WorkingDays.Where(d => d.Day==DateTime.Now.DayOfWeek
                &&DateTime.Now.TimeOfDay.CompareTo(d.CloseAt)<=0
                &&DateTime.Now.TimeOfDay.CompareTo(d.OpenAt)>0)
                .Count()>0),

                WorkingTime=d.DoctorInfo.WorkingDays.Where(d => d.Day==DateTime.Now.DayOfWeek).Select(d =>
                new WorkingDayResponseDto
                {
                    Day=d.Day.ToString(),
                    CloseAt=d.CloseAt,
                    OpenAt=d.OpenAt
                })
                .FirstOrDefault(),

                ClinicSchedule = d.DoctorInfo.WorkingDays.Select(d =>
                new WorkingDayResponseDto
                {
                    Day=d.Day.ToString(),
                    CloseAt=d.CloseAt,
                    OpenAt=d.OpenAt
                }).ToList()

            }).Where(d=>d.IsWorking==true).ToList();
            return Ok(doctorDTOs);
        }
        /*[HttpGet]
        public async Task<IActionResult> GetDoctors()
        {

            var doctors = await _context.Users.Where(u => u.DoctorInfo != null)
                .Include(u => u.DoctorInfo)
                .ThenInclude(u => u.WorkingDays)
                .ToListAsync();


            var doctorDTOs = doctors.Select(d => new
            {
                UserId = d.Id,
                d.FirstName,
                d.LastName,
                d.DoctorInfo.Specialization,
                d.ProfilePicture,
                Rating = Math.Round(5 * Random.Shared.Next(90, 100)*0.01, 2)
            }).ToList();

            return Ok(doctorDTOs);
        }*/
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {

            var doctors = await _context.Users.Where(u => u.DoctorInfo != null)
                .Include(u => u.DoctorInfo)
                .ThenInclude(u => u.WorkingDays)
                .ToListAsync();
            if (!doctors.Any())
            {
                return BadRequest();
            }
            var doctorDTOs = doctors.Select(d => new DoctorDetailResponseDto
            {
                Email = d.Email,
                FirstName = d.FirstName,
                LastName  = d.LastName,
                Specialization=d.DoctorInfo.Specialization,
                Description = d.DoctorInfo.Description,
                Services=d.DoctorInfo.Services.Split(","),
                Address = d.Address,
                Rating=Math.Round(5 * Random.Shared.Next(90, 100)*0.01, 2),
                ClinicFees=d.DoctorInfo.ClinicFees,
                Experience=d.DoctorInfo.Experience,
                Latitude=d.Latitude,
                Longitude=d.Longitude,
                PhoneNumber=d.PhoneNumber,
                ProfilePicture = d.ProfilePicture,
                UserId=d.Id,
                IsWorking=(d.DoctorInfo.WorkingDays.Where(d => d.Day==DateTime.Now.DayOfWeek
                &&DateTime.Now.TimeOfDay.CompareTo(d.CloseAt)<=0
                &&DateTime.Now.TimeOfDay.CompareTo(d.OpenAt)>0)
                .Count()>0),

                WorkingTime=d.DoctorInfo.WorkingDays.Where(d => d.Day==DateTime.Now.DayOfWeek).Select(d =>
                new WorkingDayResponseDto
                {
                    Day=d.Day.ToString(),
                    CloseAt=d.CloseAt,
                    OpenAt=d.OpenAt
                })
                .FirstOrDefault(),

                ClinicSchedule = d.DoctorInfo.WorkingDays.Select(d =>
                new WorkingDayResponseDto
                {
                    Day=d.Day.ToString(),
                    CloseAt=d.CloseAt,
                    OpenAt=d.OpenAt
                }).ToList()

            });
            return Ok(doctorDTOs);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDoctor(string Id)
        {
            var doctors = await _context.Users.Where(u => u.DoctorInfo != null && u.Id==Id)
                .Include(u => u.DoctorInfo)
                .ThenInclude(u => u.WorkingDays)
                .ToListAsync();
            if (!doctors.Any())
            {
                return BadRequest();
            }
            var doctorDTO = doctors.Select(d => new DoctorDetailResponseDto
            {
                Email = d.Email,
                FirstName = d.FirstName,
                LastName  = d.LastName,
                Specialization=d.DoctorInfo.Specialization,
                Description = d.DoctorInfo.Description,
                Services=d.DoctorInfo.Services.Split(","),
                Address = d.Address,
                Rating=Math.Round(5 * Random.Shared.Next(90, 100)*0.01, 2),
                ClinicFees=d.DoctorInfo.ClinicFees,
                Experience=d.DoctorInfo.Experience,
                Latitude=d.Latitude,
                Longitude=d.Longitude,
                PhoneNumber=d.PhoneNumber,
                ProfilePicture = d.ProfilePicture,
                UserId=d.Id,
                IsWorking=(d.DoctorInfo.WorkingDays.Where(d => d.Day==DateTime.Now.DayOfWeek
                &&DateTime.Now.TimeOfDay.CompareTo(d.CloseAt)<=0
                &&DateTime.Now.TimeOfDay.CompareTo(d.OpenAt)>0)
                .Count()>0),

                WorkingTime=d.DoctorInfo.WorkingDays.Where(d => d.Day==DateTime.Now.DayOfWeek).Select(d =>
                new WorkingDayResponseDto
                {
                    Day=d.Day.ToString(),
                    CloseAt=d.CloseAt,
                    OpenAt=d.OpenAt
                })
                .FirstOrDefault(),

                ClinicSchedule = d.DoctorInfo.WorkingDays.Select(d =>
                new WorkingDayResponseDto
                {
                    Day=d.Day.ToString(),
                    CloseAt=d.CloseAt,
                    OpenAt=d.OpenAt
                }).ToList()

            });
            return Ok(doctorDTO);
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchDoctors(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Search parameter is required.");
            }

            var doctors = await _context.Users
                .Where(u => u.DoctorInfo != null && (u.FirstName.ToLower().Contains(name) || u.LastName.ToLower().Contains(name)))
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
                .Where(d => d.FirstName.ToLower().StartsWith(name))
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
                .Where(d => !d.FirstName.ToLower().StartsWith(name) && (d.FirstName.ToLower().Contains(name) || d.LastName.ToLower().Contains(name)))
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
            if (doctorDTOs.Count == 0)
            {
                return NotFound($"No doctor found with name containing '{name}'.");
            }
            return Ok(doctorDTOs);
        }
        [HttpPost("register")]
        public async Task<IActionResult> DoctorRegisterAsync([FromForm] DoctorRegisterDto model, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model, lang, "Doctor");

            if (!result.IsAuthenticated)
            {
                return BadRequest(new { result.Message, result.Errors, status = false });
            }


            var doctorInfo = new DoctorInfo
            {
                UserId = (await _userManager.FindByEmailAsync(model.Email.Trim())).Id,
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
                await _context.DoctorWorkingDays.AddAsync(new DoctorWorkingDay { DoctorInfoId = doctorInfo.Id, Day = day.Day, OpenAt=day.OpenAt, CloseAt=day.CloseAt });
            }

            await _context.SaveChangesAsync();

            return Ok(result);
        }
        [HttpPut("update-profile"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateProfile([FromForm] DoctorRegisterDto profile, [FromHeader] string? lang = "en")
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
            user.FirstName = profile.FirstName.Trim();
            user.LastName = profile.LastName.Trim();
            user.BirthDate = profile.BirthDate;
            user.PhoneNumber = profile.PhoneNumber;
            user.Governorate = profile.Governorate;
            user.Latitude = profile.Latitude;
            user.Longitude = profile.Longitude;
            user.Address = profile.Address?.Trim();

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
            doctorInfo.Description = profile.Description.Trim();
            doctorInfo.ClinicFees = profile.ClinicFees;
            doctorInfo.Services = string.Join(",", profile.Services);
            doctorInfo.Specialization = profile.Specialization.Trim();

            _context.DoctorInfos.Update(doctorInfo);

            _context.DoctorWorkingDays.RemoveRange(doctorInfo.WorkingDays);
            foreach (var day in profile.WorkingDays)
            {
                _context.DoctorWorkingDays.Add(new DoctorWorkingDay { DoctorInfoId = doctorInfo.Id, Day = day.Day, OpenAt=day.OpenAt, CloseAt=day.CloseAt });
            }

            await _context.SaveChangesAsync();
            var profileResponse = new DoctorResponseDto
            {
                Email= user.Email,
                FirstName = user.FirstName,
                LastName= user.LastName,
                Address= user.Address,
                Latitude = user.Latitude,
                Longitude= user.Longitude,
                BirthDate= user.BirthDate,
                PhoneNumber=user.PhoneNumber,
                ProfilePicture =user.ProfilePicture,
                ClinicFees= profile.ClinicFees,
                Description= profile.Description,
                WorkingDays=profile.WorkingDays.Select(c => new WorkingDayResponseDto { CloseAt=c.CloseAt, OpenAt=c.OpenAt, Day=c.Day.ToString() }).ToList(),
                Experience= profile.Experience,
                Services=profile.Services,
                Specialization=profile.Specialization,
                UserId=user.Id
            };
            return Ok(new { status = true, profileResponse });
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

            var userLocation = new GeoCoordinate((double)user.Latitude.Value, (double)user.Longitude.Value);
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
            }).Select(d => new DoctorDetailResponseDto
            {
                Email = d.User.Email,
                FirstName = d.User.FirstName,
                LastName  = d.User.LastName,
                Specialization=d.Specialization,
                Description = d.Description,
                Services=d.Services.Split(","),
                Address = d.User.Address,
                Rating=Math.Round(5 * Random.Shared.Next(90, 100)*0.01, 2),
                ClinicFees=d.ClinicFees,
                Experience=d.Experience,
                Latitude=d.User.Latitude,
                Longitude=d.User.Longitude,
                PhoneNumber=d.User.PhoneNumber,
                ProfilePicture = d.User.ProfilePicture,
                UserId=d.User.Id,
                IsWorking=(d.WorkingDays.Where(d => d.Day==DateTime.Now.DayOfWeek
                &&DateTime.Now.TimeOfDay.CompareTo(d.CloseAt)<=0
                &&DateTime.Now.TimeOfDay.CompareTo(d.OpenAt)>0)
                .Count()>0),

                WorkingTime=d.WorkingDays.Where(d => d.Day==DateTime.Now.DayOfWeek).Select(d =>
                new WorkingDayResponseDto
                {
                    Day=d.Day.ToString(),
                    CloseAt=d.CloseAt,
                    OpenAt=d.OpenAt
                })
                .FirstOrDefault(),

                ClinicSchedule = d.WorkingDays.Select(d =>
                new WorkingDayResponseDto
                {
                    Day=d.Day.ToString(),
                    CloseAt=d.CloseAt,
                    OpenAt=d.OpenAt
                }).ToList()

            }).ToList();

            return Ok(new
            {
                message = lang == "ar" ? "تم العثور على الأطباء القريبين" : "Nearby doctors found",
                doctors = nearbyDoctors
            });
        }

    }
}
