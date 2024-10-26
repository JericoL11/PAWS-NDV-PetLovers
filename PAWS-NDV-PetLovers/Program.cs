using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --Database Injection -  Injecting dbContext into our APP (based on appSettings)
builder.Services.AddDbContext<PAWS_NDV_PetLoversContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("PAWS-NDV-PetLoversContext") ??
   // -- for error message
   throw new InvalidOperationException("Connection string not found 'PAWS-NDV-PetLoversContext' not found")));


// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set your desired session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});

// Other service registrations...
builder.Services.AddHttpContextAccessor(); // register IHttpContextAccessor 

builder.Services.AddScoped<AuthFilter>(); // register AuthFilter


var app = builder.Build();

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


// Add session and authorization middlewares
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Logins}/{action=Login}/{id?}");

app.Run();
