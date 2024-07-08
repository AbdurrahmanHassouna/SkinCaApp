using APIdemo.DTOs;
using APIdemo.Auth;
using APIdemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using OtpNet;
using System;
using Microsoft.AspNetCore.DataProtection;
using APIdemo.Tools;

namespace APIdemo.Services
{
    public class AuthService : IAuthService
    {
        public const string ResetPasswordTokenPurpose = "ResetPassword";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly JWT _jwt;
        private readonly IdentityOptions _options;
        private AppDbContext _context;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt
            , IEmailSender emailSender, IOptions<IdentityOptions> options, AppDbContext context)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _emailSender = emailSender;
            _options = options?.Value ?? new();
            _context = context;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model,string? lang,string? role ="User")
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel
                {
                    Message = (lang == "ar") ? "الحساب مسجل بالفعل" : "Email is already registered"
                };
            }

            var user = new ApplicationUser
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                BirthDate =(model.BirthDate != DateTime.MinValue)?model.BirthDate : null,
                Address = model.Address?.Trim(),
                Governorate = model.Governorate,
                Latitude=model.Latitude,
                Longitude=model.Longitude
            };
            if (model.ProfilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ProfilePicture.CopyToAsync(memoryStream);
                    user.ProfilePicture = memoryStream.ToArray();
                }
                if (user.ProfilePicture != null)
                {
                    string? type = ImageTools.GetImageType(user.ProfilePicture);
                    if (type == null)
                    {
                        return new AuthModel { IsAuthenticated= false ,Message = "unsupported picture type" };
                    }
                }
            }
            var result = await _userManager.CreateAsync(user, model.Password.Trim());
            if (!result.Succeeded)
            {
                return new AuthModel { IsAuthenticated = false,Errors = result.Errors.Select(e => e.Description).ToList(),Message= "Unable to Register the User"};
            }
            await _userManager.AddToRoleAsync(user,role);
           
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                UserName= user.FirstName+" "+user.LastName,
                ProfilePicture = user.ProfilePicture,
                Message = (lang == "ar") ? "تم التسجيل بنجاح" : "Email is registered successfully",
                Email = user.Email,
                IsDoctor =await _userManager.IsInRoleAsync(user, "Doctor"),
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            
            foreach (var sRole in roles)
                roleClaims.Add(new Claim("roles", sRole));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            }.Union(roleClaims).Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials
             );
            return jwtSecurityToken;
        }
        public async Task<AuthModel> GetTokenAsync(LoginModel model, string? lang = "en")
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return new AuthModel
                {
                    
                    Message = (lang == "ar") ? "الحساب غير مسجل" : "Email is not registered",
                    Errors = new List<string> { (lang == "ar") ? "الحساب غير مسجل" : "Email is not registered" },
                    IsAuthenticated=false
                };
            }
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new AuthModel
                {
                    Message = (lang == "ar") ? "كلمة مرور خاطئة" : "Password is not Correct",
                    IsAuthenticated=false
                };
            }
            /*if (!await _userManager.IsEmailConfirmedAsync(user))
                return new AuthModel
                {
                    Message = (lang == "ar") ? "الحساب غير موثق" : "Email not confirmed"
                };*/
            var JwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                UserName = user.FirstName,
                ProfilePicture = user.ProfilePicture,
                Message = (lang == "ar") ? "تم التسجيل بنجاح" : "Successful Login",
                Email = user.Email,
                ExpiresOn = JwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                IsDoctor = await _userManager.IsInRoleAsync(user,"Doctor"),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken)
            };
        }
        //return all claims
        /*public async Task<IDictionary<string,object>?> GetValidatedToken(string Header)
        {
            string token;
            if (Header.StartsWith("Bearer "))
            {
                token = Header.ToString().Substring("Bearer ".Length);
            }
            else return null;
            var result = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidAudience = _jwt.Audience,
                ValidIssuer = _jwt.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key))
            });
            if(!result.IsValid) return null;

            return result.Claims;
        }*/
        public async Task<string?> GetUserIdAsync(string Header)
        {
            string token;
            if (Header.StartsWith("Bearer "))
            {
                token = Header.ToString().Substring("Bearer ".Length);
            }
            else return null;
            var result = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidAudience = _jwt.Audience,
                ValidIssuer = _jwt.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key))
            });
            if (!result.IsValid) return null;

            return result.Claims["userid"].ToString();
        }
        public async Task<string?> SendForgotPasswordEmail(string email, string? lang = "en")
        {
            var user = await _userManager.FindByEmailAsync(email);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (lang == "ar")
            {
                await _emailSender.SendEmailAsync(
               user.Email,
               "إعادة تعيين كلمة المرور",
               $"رمز إعادة التعيين {code}");

                return "تم إرسال الرمز عبر البريد الإلكتروني";
            }
            else
            {
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Reset Password",
                    $" reset code : {code}");
                Console.WriteLine($"reset password code : {code}");
                return "Email Sent";
            }
        }
        public async Task<bool> VerifyResetPasswordCode(string code, ApplicationUser user)
        {
            var result = await _userManager.VerifyUserTokenAsync(user,
                 _options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, code);
            return result;
        }
        public async Task<string?> SendEmailConfirmation(string Email, string lang)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (await _userManager.IsEmailConfirmedAsync(user))
                return (lang == "ar") ? "لقد تم توثيق هذا الحساب بالفعل" : "Email already confirmed";


            /* var totp = new Totp(Encoding.ASCII.GetBytes(user.EmailSecretKey), step: 60);
             var code = totp.ComputeTotp(DateTime.Now);*/
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (lang == "ar")
            {
                /*  string url =$"http://skinca.runasp.net/Account/ConfirmEmail?code={code}&email={Email}";*/
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "توثيق البريد الإلكتروني",
                    $"رمز التحقق هو {code}");
                return "تم إرسال البريد الإلكتروني";
            }
            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm Email",
                $"confirmtion code is  {code}");
            return "Email Sent";

        }
        /*public bool VerifyTotp(string code, string secret)
        {
           

            return 
        }*/
    }
}
