using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shopping.Enums;
using Shopping.Models.Domain;
using Shopping.Models.DTO;
using Shopping.Models.ViewModels;
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
            return View("Login", new Vm_LoginRegister() { IsRegister = true});
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

            if (string.IsNullOrEmpty(model.Role))
            {
                model.Role = "User";
            }

            var result = await _authService.RegisterAsync(model);

            /*return Ok(result.Message);*/
            if (result.StatusCode == 1)
            {
                return Json(new { from = "register", HttpStatusCode = 200});
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Json(new { Message = result.Message ?? "Registration failed", HttpStatusCode = HttpStatusCode.Unauthorized });
            }
        }

        [HttpGet]
        public IActionResult GetWelcomePopup()
        {
            return PartialView("_WelcomePartial");
        }
        
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new Vm_LoginRegister() { ReturnUrl = ReturnUrl };
            if (ReturnUrl != null && ReturnUrl.Contains("Supplier"))
            {
                return View("SupplierLogin", model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SupplierLogin(Vm_LoginRegister model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model.Email is null || model.Password is null)
            {
                return View(model);
            }

            try
            {
                var result = await _authService.LoginAsync(model);
                if (result.StatusCode == 1)
                {
                    // Get the user
                    var user = await _userManager.FindByNameAsync(model.Email);

                    //Get the role
                    var roles = await _userManager.GetRolesAsync(user);

                    // Create the claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.Name),
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
                    return Json(new { Message = result.Message ?? "Could not logged in", HttpStatusCode = HttpStatusCode.Unauthorized });
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
        public IActionResult GoogleLogin(UserRole role, string returnUrl)
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account", new { role = role, returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(UserRole role, string returnUrl)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return PartialView("_LoginPartial");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false); // is present => false
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value }; //principal => gets or sets security claims
            if (!result.Succeeded)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Name = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (!identResult.Succeeded)
                {
                    return AccessDenied();
                }

                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }

                if (await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _userManager.AddToRoleAsync(user, role.ToString());
                }

                identResult = await _userManager.AddLoginAsync(user, info);
                if (identResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    //return RedirectToAction("Index", "Home");
                }
            }
            if (returnUrl != null && returnUrl.Contains("Supplier"))
            {
                return RedirectToAction("Index", "Supplier");
            }
            return RedirectToAction("Index", "Home");

        }
    }

}
