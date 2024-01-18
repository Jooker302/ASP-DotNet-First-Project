using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VPProject.Models;

namespace VPProject.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly VpprojectContext _dbContext;


    public AuthController(ILogger<HomeController> logger, VpprojectContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    public IActionResult Index()
    {
        return View();
    }

    

    [HttpPost]
    [ValidateAntiForgeryToken]
     public IActionResult Login(string email, string password)
    {
        using (var context = new VpprojectContext())
        {
            var user = context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
              
                if (user.Type == "Admin")
                {
                    // ViewData["UserName"] = user.Name;
                    HttpContext.Session.SetString("UserType", user.Type);
                    HttpContext.Session.SetString("UserName", user.Name);
                   HttpContext.Session.SetString("UserId", user.Id.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else if (user.Type == "Patient")
                {
                    HttpContext.Session.SetString("UserType", user.Type);
                     HttpContext.Session.SetString("UserName", user.Name);
                     HttpContext.Session.SetString("UserId", user.Id.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        return RedirectToAction("Index");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            using (var dbContext = new VpprojectContext())
            {
                // Check if the email is unique
                if (dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return View();
                }

                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index", "Home"); // Redirect to login page after registration
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Auth");
    }

     protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
        base.Dispose(disposing);
    }
}
