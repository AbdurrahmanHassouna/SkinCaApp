using SkinCaApp.DTOs;
using SkinCaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace SkinCaApp.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model,string? lang,string? role="User");
        Task<AuthModel> GetTokenAsync(LoginModel model,string? lang);
        /*Task<IDictionary<string,object>?> GetValidatedToken(string Header);*/
        Task<string?> GetUserIdAsync(string Header);
        Task<string?> SendForgotPasswordEmail(string email,string lang);
        Task<bool> VerifyResetPasswordCode(string token, ApplicationUser user);
        Task<string?> SendEmailConfirmation(string email,string lang);
        /*public bool VerifyTotp(string code, string secret);*/
    }
}
