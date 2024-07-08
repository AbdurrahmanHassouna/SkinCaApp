using APIdemo.DTOs;
using APIdemo.Models;
using APIdemo.Services;
using APIdemo.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;

namespace APIdemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _context;
        public AccountController(IAuthService authService, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger, AppDbContext appDbContext)
        {
            _context= appDbContext;
            _authService = authService;
            _userManager = userManager;
            _logger=logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModel model, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model, lang);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /* [HttpPost("doctor-register")]
         public async Task<IActionResult> DoctorRegisterAsync([FromForm] DoctorRegisterDto model, [FromHeader] string? lang = "en")
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             var result = await _authService.RegisterAsync(model, lang);

             if (!result.IsAuthenticated)
             {
                 return BadRequest(new { result.Message, result.Errors, status = false });
             }
             var user =await _userManager.FindByEmailAsync(model.Email);
             await _userManager.AddToRoleAsync(user, "Doctor");

             var doctorInfo = new DoctorInfo
             {
                 UserId = user.Id,
                 Experience = model.Experience,
                 ClinicFees = model.ClinicFees,
                 Description = model.Description,
                 Services = string.Join(",",model.Services),
                 Specialization = model.Specialization
             };
             await _context.DoctorInfos.AddAsync(doctorInfo);
             await _context.SaveChangesAsync();

             return Ok(result);
         }
        */
        [HttpPost("get-token")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel model, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.GetTokenAsync(model, lang);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("get-user-id")]
        [Authorize]
        public async Task<IActionResult> GetUserId()
        {
            /*var Authorization = Request.Headers.Authorization;
            var Id = await _authService.GetUserIdAsync(Authorization);
            */
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new { Error = "user not found" });
            }
            return Ok(new { user.Id });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassowrd(EmailModel model, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return (lang=="ar") ? NotFound(new { message = "هذاالحساب غير مسجل", status = false })
                    : NotFound(new { message = "Email not registered", status = false });
            }
            var result = await _authService.SendForgotPasswordEmail(model.Email, lang);
            if (result is null) return BadRequest("Error");
            return Ok(new { message = result });
        }
        [HttpPost("verify-reset-password-code")]
        public async Task<IActionResult> VerifyResetPasswordCode(VerfiyEmailModel model, [FromHeader] string? lang = "en")
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return (lang == "ar") ?
                     NotFound(new { message = $"لم يتم إيجاد مستخدم بهذا الحساب {model.Email}", status = false })
                     : NotFound(new { message = $"User with {model.Email} Not Found", status = false });
            }

            var result = await _authService.VerifyResetPasswordCode(model.Code, user);
            return Ok(new { status = result });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = ModelState.IsValid, error = ModelState["errors"]?.RawValue??"no errors" });
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return (lang == "ar") ?
                    NotFound(new { message = $"لم يتم إيجاد مستخدم بهذا الحساب {model.Email}", status = false })
                    : NotFound(new { message = $"User with {model.Email} Not Found", status = false });

            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            return Ok(new { status = result.Succeeded, result.Errors });
        }
        [HttpGet("send-email-confirmation/{email}")]
        public async Task<IActionResult> SendEmailConfirmationCode(string email, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (lang=="ar") ? NotFound(new { message = "هذاالحساب غير مسجل", status = false })
                    : NotFound(new { message = "Email not registered", status = false });
            }

            var result = await _authService.SendEmailConfirmation(email, lang);

            return Ok(new { message = result });
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(VerfiyEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Email not registered");
            }
            var result = await _userManager.ConfirmEmailAsync(user, model.Code);
            /*var result = _authService.VerifyTotp(code, user.EmailSecretKey);*/

            return Ok(new { status = result.Succeeded });
        }
        [HttpGet("profile"), Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user== null)
            {
                return NotFound(new { message = "User Not Found" });
            }

            ProfileResponseDto profile = new ProfileResponseDto
            {
                Email       = user.Email,
                FirstName   = user.FirstName,
                LastName    = user.LastName,
                Address     = user.Address,
                Latitude    = user.Latitude,
                Longitude   = user.Longitude,
                BirthDate   = user.BirthDate,
                PhoneNumber = user.PhoneNumber
            };
            string? type = null;
            if (user.ProfilePicture != null)
            {
                type = ImageTools.GetImageType(user.ProfilePicture);
            }

            return Ok(new { profile, type, profilePicture = user.ProfilePicture });
        }
        [HttpPut("update-profile"), Authorize]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileDTO model, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new { status = false, message = (lang=="ar") ? "لم يتم العثور علي مستخدم" : "User not found" });
            }
            user.Address= (model.Address!=null)?model.Address:user.Address;
            user.FirstName= model.FirstName;
            user.LastName= model.LastName;
            user.Address = model.Address;
            user.BirthDate= model.BirthDate;
            user.PhoneNumber=model.PhoneNumber;
            user.Governorate = model.Governorate;
            user.Latitude= model.Latitude;
            user.Longitude= model.Longitude;

            if (model.Email.ToUpper()!=user.NormalizedEmail)
            {
                if ((await _userManager.FindByEmailAsync(model.Email))!= null)
                {
                    return BadRequest(new { status = false, message = (lang=="ar") ? "الحساب مستخدم بالفعل" : "Email is already used" });
                }
                user.Email= model.Email;
                user.UserName= model.Email;
                user.EmailConfirmed = false;
            }
            if (model.ProfilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ProfilePicture.CopyToAsync(memoryStream);
                    user.ProfilePicture = memoryStream.ToArray();
                }
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var profile = new ProfileResponseDto
                {
                    Email= user.Email,
                    FirstName = user.FirstName,
                    LastName= user.LastName,
                    Address= user.Address,
                    Latitude = user.Latitude,
                    Longitude= user.Longitude,
                    BirthDate= user.BirthDate,
                    PhoneNumber=user.PhoneNumber,
                    ProfilePicture =user.ProfilePicture
                };
                return Ok(new { status = result.Succeeded, profile });
            }
            return BadRequest(new {status=false,Message="unable to update profile"});
        }
        [HttpPut("change-password"), Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model, [FromHeader] string? lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(new { status = false, message = (lang=="ar") ? "لم يتم العثور علي مستخدم" : "User not found" });
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            return Ok(new
            {
                status = result.Succeeded,
                message = (result.Succeeded) ?
                    ((lang=="ar") ? "تم تغيير الرقم السري" : "password changed") :
                    ((lang=="ar") ? "لم يتم تغيير الرقم السري" : "password didn't changed"),
                result.Errors
            });
        }
 /*       [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleResponse)) };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result?.Principal != null)
            {
               
            }

            return BadRequest();
        }
*/
        [HttpPut("profile-picture"), Authorize]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                user.ProfilePicture = memoryStream.ToArray();
            }
            if (user.ProfilePicture != null)
            {
                string? type = ImageTools.GetImageType(user.ProfilePicture);
                if (type == null)
                {
                    return BadRequest("unavailable type");
                }
            }

            await _userManager.UpdateAsync(user);


            return Ok(new { status = true, message = "Profile picture uploaded successfully.",profilePicture = user.ProfilePicture});
        }
        [HttpDelete("delete-account"), Authorize]
        public async Task<IActionResult> DeleteAccount([FromHeader] string? lang = "en")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new { status = false, message = (lang=="ar") ? "لم يتم العثور علي مستخدم" : "User not found" });
            }
            await _userManager.DeleteAsync(user);
            return NoContent();
        }


    }
}

