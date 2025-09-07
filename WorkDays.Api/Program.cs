using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using WorkDays.Api.Interfaces;
using WorkDays.Api.Services;
using WorkDays.Data;
using WorkDays.Data.Interfaces;
using WorkDays.Data.Models;
using WorkDays.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
// Add support for enum as string in JSON
//builder.Services.AddControllers().AddJsonOptions(options =>
  //          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Endpoints API explorer
builder.Services.AddEndpointsApiExplorer();

// DI
builder.Services.AddScoped<IWorkDayRepository, WorkDayRepository>();
builder.Services.AddScoped<IBaseRepository<WorkDay>, BaseRepository<WorkDay>>();
builder.Services.AddScoped<IWorkDayService, WorkDayService>();

// --- HEROKU SPECIFIC SETTINGS ---
// Následující sekce je určena pouze pro nasazení na Heroku.
// Pro lokální vývoj ponechte zakomentované!
//
// builder.Services.AddRazorPages();
// var isHeroku = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DYNO"));
// builder.Services.Configure<ForwardedHeadersOptions>(options =>
// {
//     options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
//     if (isHeroku)
//     {
//         options.KnownNetworks.Clear();
//         options.KnownProxies.Clear();
//     }
// });
// builder.Services.AddHttpsRedirection(options =>
// {
//     if (isHeroku)
//     {
//         options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
//         options.HttpsPort = 443;
//     };
// });
// builder.Services.AddDbContext<AppDbContext>(options =>
// {
//     // Heroku poskytuje connection string v proměnné DATABASE_URL
//     var match = Regex.Match(Environment.GetEnvironmentVariable("DATABASE_URL") ?? "", @"postgres://(.*):(.*)@(.*):(.*)/(.*)");
//     options.UseNpgsql($"Server={match.Groups[3]};Port={match.Groups[4]};User Id={match.Groups[1]};Password={match.Groups[2]};Database={match.Groups[5]};sslmode=Prefer;Trust Server Certificate=true");
// });
// --- END HEROKU SPECIFIC SETTINGS ---


var app = builder.Build();

// Configure the HTTP request pipeline and forwarded headers for Heroku
app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseRouting();

//app.MapRazorPages();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Program>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    logger.LogInformation("Create database if not exists");
   // db.Database.EnsureCreated();
   //var connection = db.Database.GetDbConnection();
    logger.LogInformation("Database connected");
   // await connection.OpenAsync();
    if (!db.WorkDays.Any())
    {
        db.WorkDays.Add(new WorkDay
        {
            Date = DateTime.SpecifyKind(new DateTime(2025, 9, 6, 1, 0, 0), DateTimeKind.Utc),
            StartTime = new TimeOnly(10, 30),
            EndTime = new TimeOnly(18, 0),
            Break = new TimeOnly(0, 30),
            IsHoliday = false,
            Type = DayType.Regular,
        });
        
        db.SaveChanges();
        db.Database.Migrate();

        logger.LogInformation("Database created");
    }
}


app.Run();
