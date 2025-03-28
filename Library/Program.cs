using AspNetCore.Unobtrusive.Ajax;
using Library.Data;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<LibraryUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()  // Toe te voegen om met rollen te kunnen werken
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Taal-resources toekennen aan de services
builder.Services.AddLocalization(options => options.ResourcesPath = "LanguageResources");
builder.Services.AddMvc()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
builder.Services.AddControllersWithViews();

// MailKit als service toevoegen
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();

// Add API services
builder.Services.AddControllers();

// Add to use Ajax
builder.Services.AddUnobtrusiveAjax();

var app = builder.Build();
Globals.App = app;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Voorzie het gebruik van Ajax in de request-afhandeling
app.UseUnobtrusiveAjax();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.MapRazorPages();

// Toegevoegd voor het seeden van de database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
    var userManager = services.GetRequiredService<UserManager<Library.Models.LibraryUser>>();
    ContextSeeder.Initialize(context, userManager);
}

app.Run();
