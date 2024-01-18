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
        // Use a using statement to ensure proper disposal of the context
        using (var context = new VpprojectContext())
        {
            // Check if the email and password match a user in the database
            var user = context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Redirect based on user type
                if (user.Type == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.Type == "Patient")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // If no matching user found, redirect to login page or show an error message
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

     protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
        base.Dispose(disposing);
    }
}
