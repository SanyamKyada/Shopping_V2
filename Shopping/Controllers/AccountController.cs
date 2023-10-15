using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shopping.Models.Domain;
using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUserAuthenticationService _authService;
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        public AccountController(IConfiguration config, IUserAuthenticationService authService, DatabaseContext context,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _config = config;
            _authService = authService;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            /*  We will create a user with admin rights, after that we are going to 
             *  comment this method because we need only one user in this application
             *  And, if you need other users then you can implement this method with view   
             */
            /*var model = new RegistrationModel
            {
                Email = "admin@gmail.com",
                Username = "admin",
                Password = "Admin@123",
                PasswordConfirm = "Admin@123",
                Name = "Sanyam",
                Role = "Admin"
            };*/
            // if you want to register with user, chnage Role="User"

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.RegisterAsync(model);

            /*return Ok(result.Message);*/
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = "Registration Failed..";
                return RedirectToAction(nameof(Login));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetLoginPopup()
        {
            return PartialView("_LoginPartial");
        }
        [HttpGet]
        public IActionResult GetWelcomePopup()
        {
            return PartialView("_WelcomePartial");
        }
        
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginModel() {ReturnUrl = ReturnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model.Username is null && model.Password is null)
            {
                return View(model);
            }
            HttpContext.Session.SetString("user", model.Username);

            try
            {
                var result = await _authService.LoginAsync(model);
                if (result.StatusCode == 1)
                {
                    // Get the user
                    var user = await _userManager.FindByNameAsync(model.Username);

                    //Get the role
                    var roles = await _userManager.GetRolesAsync(user);

                    // Create the claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("securityStamp", user.SecurityStamp)
                    };
                    if (roles.Contains("Admin"))
                    {
                        claims.Add(new Claim("IsAdmin", "true"));
                    }

                    // Create the JWT token
                    var jwtSettings = _config.GetSection("Jwt").Get<JwtSettings>();
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddMinutes(jwtSettings.ExpiresInMinutes);

                    var token = new JwtSecurityToken(
                        jwtSettings.Issuer,
                        jwtSettings.Audience,
                        claims,
                        expires: expires,
                        signingCredentials: credentials
                    );

                    // JWT token as a response
                    if (model.loginScenario == Enums.LoginScenario.PopupLogin)
                    {
                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token)});
                    }
                    else
                    {
                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), returnUrl = model.ReturnUrl });
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Json(new { Message = "Could not logged in..", HttpStatusCode = HttpStatusCode.Unauthorized });
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return PartialView("_LoginPartial");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false); // is present => false
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value }; //principal => gets or sets security claims
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                ApplicationUser user = new ApplicationUser
                {
                    Name = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return View(userInfo);
                    }
                }
                return AccessDenied();
            }

        }
    }

}
