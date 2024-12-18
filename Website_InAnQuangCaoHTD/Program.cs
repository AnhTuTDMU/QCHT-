using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using Website_InAnQuangCaoHTD.Controllers;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Helpers;
using Website_InAnQuangCaoHTD.Models;

var builder = WebApplication.CreateBuilder(args);

// Session and Cache setup
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>(); // Đăng ký dịch vụ Email

// Authentication and Authorization
builder.Services.AddAuthentication()
    .AddCookie("UserCookie", options =>
    {
        options.LoginPath = "/UserAccount/Login";
        options.LogoutPath = "/UserAccount/Logout";
        options.AccessDeniedPath = "/UserAccount/AccessDenied";
        options.Cookie.Name = "UserCookie";
    })
    .AddCookie("AdminCookie", options =>
    {
        options.LoginPath = "/Admin/AdminAccount/Login";
        options.LogoutPath = "/Admin/AdminAccount/Logout";
        options.AccessDeniedPath = "/Admin/AdminAccount/AccessDenied";
        options.Cookie.Name = "AdminCookie";
    });

builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<ReCaptchaSettings>(builder.Configuration.GetSection("ReCaptcha"));
// Flash Sale Service
builder.Services.AddHostedService<FlashSaleUpdateService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Route cho khu vực Admin
app.MapControllerRoute(
    name: "AdminArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
