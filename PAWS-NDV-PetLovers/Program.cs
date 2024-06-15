using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --Database Injection -  Injecting dbContext into our APP (based on appSettings)
builder.Services.AddDbContext<PAWS_NDV_PetLoversContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("PAWS-NDV-PetLoversContext") ??
   // -- for error message
   throw new InvalidOperationException("Connection string not found 'PAWS-NDV-PetLoversContext' not found")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=Index}/{id?}");

app.Run();
