using APIdemo.Auth;
using APIdemo.Authorization;
using APIdemo.Models;
using APIdemo.Services;
using APIdemo.TokenProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APIdemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var _configuration = builder.Configuration;


            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = false;

            });

            builder.Services.Configure<JWT>(_configuration.GetSection("JWT"));
            builder.Services.Configure<NetworkSecrets>(_configuration.GetSection("NetworkSecrets"));

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SkinCa"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider = "Custom";
                options.Tokens.PasswordResetTokenProvider = "Custom";
                options.Tokens.ChangeEmailTokenProvider = "Custom";
                options.Password.RequireNonAlphanumeric = false;
                
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<CustomTokenProvider<ApplicationUser>>("Custom");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,//default is True;
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
                };
            })/*.AddGoogle(options =>
            {
                options.ClientId = "";
                options.ClientSecret = "";
            })*/;

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddRazorPages();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.Run();
        }
    }
}
