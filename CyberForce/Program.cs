using System.Configuration;
using System.Security.Claims;
using CyberForce.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = "auth";
                        options.LoginPath = "/Home/LogIn";
                    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
                          policy.RequireClaim("Role", "Admin")
                                .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                              );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

