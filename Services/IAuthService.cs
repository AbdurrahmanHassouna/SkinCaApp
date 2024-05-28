using APIdemo.DTOs;
using APIdemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APIdemo.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model,string? lang);
        Task<AuthModel> GetTokenAsync(LoginModel model,string? lang);
        /*Task<IDictionary<string,object>?> GetValidatedToken(string Header);*/
        Task<string?> GetUserIdAsync(string Header);
        Task<string?> SendForgotPasswordEmail(string email,string lang);
        Task<bool> VerifyResetPasswordCode(string token, ApplicationUser user);
        Task<string?> SendEmailConfirmation(string email,string lang);
        /*public bool VerifyTotp(string code, string secret);*/
    }
}
