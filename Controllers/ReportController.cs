using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VPProject.Models;

namespace VPProject.Controllers;

public class ReportController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ReportController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<Report> reports;

        using (var dbContext = new VpprojectContext())
        {
            reports = dbContext.Reports
        .Select(r => new Report
        {
            Id = r.Id,
            UserId = r.UserId,
            Issue = r.Issue,
            IssueDate = r.IssueDate,
            Suggestion = r.Suggestion ?? string.Empty, // Handle null as an empty string
            SuggestionDate = r.SuggestionDate
        })
        .ToList();
        }

        return View(reports);
    }


    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Report report)
    {
        report.SuggestionDate = null;
        report.Suggestion = null;

        report.UserId = int.Parse(HttpContext.Session.GetString("UserId"));
        using (var dbContext = new VpprojectContext())
        {
            dbContext.Reports.Add(report);
            dbContext.SaveChanges();
        }

        return RedirectToAction("Index", "Home");

    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
