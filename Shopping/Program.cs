using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Models.Domain;
using Shopping.Repositories.Infrastructure;
using Shopping.Repositories.Services;
using static Shopping.Models.Domain.DatabaseContexct;
// usings for jwt authentication
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


//  <-- JWT + Google Login --> //
var key = Encoding.ASCII.GetBytes("This is my security key:Sem@362002");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Shopping",
            ValidAudience = "/UserAuthentication/Login",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    })
    .AddGoogle(googleOptions =>
    {
    googleOptions.ClientId = "47352457855-2fpvrg2jb9v16trtg8b7v59er8ne7c7g.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
    });

// Add session support
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.
     GetConnectionString("ShoppingConnectionStringNew")));

// For Identity // <----> //
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
     .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>(); // Registerd out repository
builder.Services.AddScoped<IMenuService, MenuService>(); // Registerd out repository
builder.Services.AddScoped<ICartService, CartService>(); // Registerd out repository
builder.Services.AddScoped<ISearchService, SearchService>(); // Registerd out repository
builder.Services.AddScoped<IProductService, ProductService>(); // Registerd out repository
builder.Services.AddTransient(typeof(IAdminService<>), typeof(AdminService<>)); ; // Registerd out repository

//  <----> //
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.Name = ".AspNetCore.Cookies";
//        options.LoginPath = "/Account/Login";
//    });

//builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/UserAuthentication/Login"); // <----> //

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;

    options.MinimumSameSitePolicy = SameSiteMode.None;
});

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

app.UseCookiePolicy();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
