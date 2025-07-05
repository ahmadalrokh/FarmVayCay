using Farm.DataAccess.Repository;
using Farm.DataAccess.Repository.IRepository;
using FarmVayCayTestOne.Data;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Farm.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using FarmVayCayTestOne.Services;
using Microsoft.CodeAnalysis.Options;

namespace FarmVayCayTestOne
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

            builder.Services.AddScoped<IEmailSender, EmialSender>();

            //For emails
            builder.Services.AddTransient<EmailService>();
           
            
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login"; // المسار الافتراضي لـ Identity
                options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // اختياري
               
            });


            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();   
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Custmor}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
