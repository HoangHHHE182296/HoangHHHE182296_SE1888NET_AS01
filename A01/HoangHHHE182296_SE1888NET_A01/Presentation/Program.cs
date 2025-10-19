using BusinessLogic.Rules;
using BusinessLogic.Services;
using BusinessLogic.Validation;
using DataAccess;
using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace Presentation {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<CategoryService>();

            builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
            builder.Services.AddScoped<SystemAccountValidator>();
            builder.Services.AddScoped<SystemAccountRules>();
            builder.Services.AddScoped<SystemAccountService>();

            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}
