using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VPProject.Models;

namespace VPProject.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public UserController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<User> users;

        using (var dbContext = new VpprojectContext())
        {
            users = dbContext.Users
        .Where(r => r.Type == "Patient")
        .Select(r => new User
        
        {
            Id = r.Id,
            Email = r.Email,
            Name = r.Name,
            Age = r.Age,
            Gender = r.Gender,
        })
        .ToList();
        }

        return View(users);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
