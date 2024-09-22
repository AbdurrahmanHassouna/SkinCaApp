using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace SkinCaApp.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<BookMark> BookMarks { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<MedicalReport> MedicalReports { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ModelResult> ModelResults { get; set; }
        public DbSet<DoctorInfo> DoctorInfos { get; set; }
        public DbSet<DoctorWorkingDay> DoctorWorkingDays { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<ApplicationUserChat> ApplicationUserChats { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Disease>()
             .HasOne(d => d.User)
             .WithMany(u => u.Diseases)
             .HasForeignKey(d => d.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BookMark>()
                .HasOne(b => b.Disease)
                .WithMany()
                .HasForeignKey(b => b.DiseaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BookMark>()
                .HasOne(b => b.User)
                .WithMany(u => u.BookMarks)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configure many-to-many relationships
            builder.Entity<Chat>()
                .HasMany(c => c.Users)
                .WithMany(u => u.Chats)
                .UsingEntity<ApplicationUserChat>(
                    j => j
                        .HasOne(uc => uc.User)
                        .WithMany(u => u.ApplicationUserChats)
                        .HasForeignKey(uc => uc.UserId)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne(uc => uc.Chat)
                        .WithMany(c => c.ApplicationUserChats)
                        .HasForeignKey(uc => uc.ChatId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey(uc => new { uc.UserId, uc.ChatId });
                    });
            

            builder.Entity<ApplicationUser>().Property(x => x.BirthDate).HasColumnType("DATE");

            string roleId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            string userId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = roleId,
                        Name = "Admin",
                        NormalizedName="ADMIN",
                        ConcurrencyStamp=roleId
                    },
                    new IdentityRole
                    {
                        Id = "d7fc4052-eaf9-4b0e-b00a-dabcfe0917e1",
                        Name = "Doctor",
                        NormalizedName="Doctor",
                        ConcurrencyStamp="d7fc4052-eaf9-4b0e-b00a-dabcfe0917e1"
                    },
                    new IdentityRole
                    {
                        Id = "f7ae0ec7-9746-4389-a8e4-1ce44265c89b",
                        Name = "User",
                        NormalizedName="USER",
                        ConcurrencyStamp="f7ae0ec7-9746-4389-a8e4-1ce44265c89b"
                    }
                );
            ApplicationUser user = new ApplicationUser
            {
                FirstName = "Super",
                LastName = "Admin",
                UserName = "SuperAdmin@TEST.COM",
                NormalizedEmail="SuperAdmin@TEST.COM",
                Email="SuperAdmin@test.com",
                NormalizedUserName="SUPERADMIN",
                Id=userId,
                EmailConfirmed=true
            };

            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = hasher.HashPassword(user, "123@Bdu456");

            builder.Entity<ApplicationUser>().HasData(
            user
                );
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = userId,
                RoleId = roleId,
            });
        }
    }
}
