using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VPProject.Models;
using Newtonsoft.Json;

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
            string userType = HttpContext.Session.GetString("UserType");

            if(userType != "Patient"){

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
            }else{
                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
// Console.WriteLine(userId);
                reports = dbContext.Reports
                .Where(r => r.UserId == userId)
            
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

    public IActionResult AddSugestion()
    {
        List<Report> reports;

        using (var dbContext = new VpprojectContext())
        {
            reports = dbContext.Reports
        .Where(r => r.SuggestionDate == null)
        .Select(r => new Report
        {
            Id = r.Id,
            UserId = r.UserId,
            Issue = r.Issue,
            IssueDate = r.IssueDate,
            Suggestion = r.Suggestion ?? string.Empty,
            SuggestionDate = r.SuggestionDate
        })
        .ToList();
        }

        return View(reports);
    }

    public IActionResult StoreSuggestion(int reportId)
    {
        ViewData["ReportId"] = reportId;
        return View();
    }

    [HttpPost]
public IActionResult StoreSuggestion(int reportId, string suggestion)
{
    Console.WriteLine($"Report with ID {reportId} not found.");
    using (var dbContext = new VpprojectContext())
    {
        var report = dbContext.Reports.Find(reportId);

string reportJson = JsonConvert.SerializeObject(report);
Console.WriteLine(reportJson);
        if (report == null)
        {
            // Log or output a message to help diagnose the issue
            Console.WriteLine($"Report with ID {reportId} not found.");
            
            // Handle the case where the report with the given id is not found
            return NotFound();
        }

        // Update the Suggestion and SuggestionDate
        if (!string.IsNullOrEmpty(suggestion))
        {
            // Only update if suggestion is not null or empty
            report.Suggestion = suggestion;
        }else{
            report.Suggestion = null;
        }

        // If you want to set SuggestionDate only when there's a new suggestion
        if (!string.IsNullOrEmpty(suggestion) && report.SuggestionDate == null)
        {
            report.SuggestionDate = DateTime.Now;
        }else{
            report.SuggestionDate = null;
        }

        

        Console.WriteLine(report);

        // Save changes to the database
        dbContext.SaveChanges();

        // Redirect to a different action or view after updating
        return RedirectToAction("Index", "Report");
    }
}



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
