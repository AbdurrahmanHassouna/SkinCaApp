using Microsoft.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OtpNet;

namespace SkinCaApp.TokenProviders
{
    public class CustomTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public CustomTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
        {
            
        }
        public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {

            var securitystamp = await manager.GetSecurityStampAsync(user);

            var totp = new Totp(Base32Encoding.ToBytes(securitystamp),step:60,totpSize:4);
            return totp.ComputeTotp();
        }
        public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            var securitystamp = await manager.GetSecurityStampAsync(user);
            
            var totp = new Totp(Base32Encoding.ToBytes(securitystamp),step:60,totpSize:4);
            
            return totp.VerifyTotp(token,out long a,new VerificationWindow(1,1));
        }
    }
}
