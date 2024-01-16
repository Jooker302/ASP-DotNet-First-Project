using Microsoft.EntityFrameworkCore;
using VPProject;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VpprojectContext>(options =>
{
    options.UseMySql("server=localhost;user=root;database=VPProject", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.28-mariadb"));
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


app.Use(async (context, next) =>
{
    try
    {
        using var dbContext = context.RequestServices.GetRequiredService<VpprojectContext>();
        await dbContext.Database.OpenConnectionAsync();
        Console.WriteLine("Database connection successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error connecting to the database: {ex.Message}");
    }

    await next();
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
