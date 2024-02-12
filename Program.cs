using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ItemsAndOrdersManagementSystem.Data;
using Microsoft.Extensions.Configuration;
using ItemsAndOrdersManagementSystem.Models;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

string connectionString;

builder.Configuration
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
