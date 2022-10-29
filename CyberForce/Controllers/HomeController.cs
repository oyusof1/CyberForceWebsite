using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberForce.Models;
using CyberForce.Services;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using FluentFTP;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
using CyberForce.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;

using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace CyberForce.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly string _connectionString;



    public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IWebHostEnvironment appEnvironment)
    {
        _logger = logger;
        _configuration = configuration;
        _appEnvironment = appEnvironment;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    #region Views
    [AllowAnonymous]
    public IActionResult Index()
    {
        //IndexViewModel vm = new();
        //string _connectionString = _configuration.GetConnectionString("DefaultConnection");
        //List<SolarArray> solarArrays = new();
        //DataService service = new DataService(_connectionString);
        //vm.solarArrays = service.GetSolarArrays();

        return View();
    }

    [AllowAnonymous]
    public IActionResult Manufacturing()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Contact()
    {
        return View(new Form());
    }

    [AllowAnonymous]
    public IActionResult ContactThankYou()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult SolarGeneration()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        var identity = (ClaimsIdentity)User.Identity;
        if (identity.IsAuthenticated)
        {
            return RedirectToAction("Index");
        } else
        {
            return View();

        }

    }

    [Authorize(Policy = "Admin")]
    public IActionResult Admin()
    {
        return View();
    }

    [HttpGet]
    [Route("/Account/AccessDenied")]
    public ActionResult AccessDenied()
    {
        return View();
    }

    #endregion

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    #region POST
    [HttpPost]
    public async Task<IActionResult> Contact(Form form)
    {
        try
        {
            if (form.File is not null)
            {
                var client = new AsyncFtpClient("127.0.0.1");
                client.Config.ValidateAnyCertificate = true;
                await client.Connect();

                var filePath = _appEnvironment.WebRootPath + $"/uploads/{form.File.FileName}";

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await form.File.CopyToAsync(stream);
                    //await client.UploadFile(filePath, $"/Users/xijkstra/Documents/{form.File.FileName}");

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                await client.Disconnect();

                var smtpClient = new SmtpClient("smtp.gmail")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("username", "password"),
                    EnableSsl = true,
                };

                smtpClient.Send(form.Email, "recipient", $"Recieved a Contact Form from {form.Name}", $"Hello, my name is {form.Name}. I would like to be contacted at {form.Phone}");

            }
            else
            {
                //var smtpClient = new SmtpClient("smtp.gmail")
                //{
                //    Port = 587,
                //    Credentials = new NetworkCredential("username", "password"),
                //    EnableSsl = true,
                //};

                //smtpClient.Send(form.Email, "recipient", $"Recieved a Contact Form from {form.Name}", $"Hello, my name is {form.Name}. I would like to be contacted at {form.Phone}");
            }


            return RedirectToAction("ContactThankYou");
        }
        catch
        {
            return RedirectToAction("Error");
        }

    }

    [HttpPost]
    [Route("SignIn")]
    public async Task<IActionResult> SignIn(Form form)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        DataService service = new DataService(_connectionString);
        var (user, loginValid) = service.ValidateUser(form.Email.Split('@')[0], form.Password);

        AdminViewModel vm = new();
        //string _connectionString = _configuration.GetConnectionString("DefaultConnection");
        //DataService service = new DataService(_connectionString);
        //vm.ftpListItems = await service.GetFtpListItems();
        //vm.messages = service.GetAllMessages("hostname", 110, false, "username", "password");

        if (!loginValid)
        {
            TempData["LoginFailed"] = $"The username or password is incorrect.";

            return RedirectToAction("Login");
        }
        else
        {
            await SignInUser(user.sAMAccountName, user.userRole);
            TempData["LoginFailed"] = "";
            if (user.userRole == "Admin")
            {
                return RedirectToAction("Admin", vm);
            }
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task SignInUser(string username, string role)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("Role", role),
            };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.User.AddIdentity(claimsIdentity);

        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);
    }
    #endregion

}