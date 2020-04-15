using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ApiProject.Models;
using ApiProject.Modelviews;
using ApiProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiProject.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDb _db;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(ApplicationDb db, UserManager<ApplicationUser> manager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _db = db;
            _manager = manager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                StringBuilder Errs = new StringBuilder();

                if (EmailExistes(model.Email))
                {
                    Errs.AppendLine("Email is used");
                    //return BadRequest("");
                }
                if (!isEmailValid(model.Email))
                {
                    Errs.AppendLine("Email is not valid !!");
                    //return BadRequest("Email is not valid !!");
                }
                if (UserNameExistes(model.UserName))
                {
                    Errs.AppendLine("UserName is used");
                    //return BadRequest("UserName is used");
                }
                if (Errs.Length > 0) return BadRequest(Errs.ToString());

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                var result = await _manager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync("User"))
                    {
                        if (!await _manager.IsInRoleAsync(user, "User"))
                        {
                            await _manager.AddToRoleAsync(user, "User");
                        }
                    }

                    var token = await _manager.GenerateEmailConfirmationTokenAsync(user);
                    //var confirmLinkAsp = Url.Action("RegisterationConfirm", "Account", new
                    //{ ID = user.Id, Token = HttpUtility.UrlEncode(token) }, Request.Scheme);

                    // to encode token for security and sending it to angular 
                    var encodeToken = Encoding.UTF8.GetBytes(token);
                    var newToken = WebEncoders.Base64UrlEncode(encodeToken);

                    var confirmLink = $"http://localhost:4200/registerconfirm?ID={user.Id}&Token={newToken}";
                    var txt = "Please Confirm Yousr Registeration";
                    var link = $"<a href=\"" + confirmLink + "\">Confirm Email Address</a>";
                    var title = "Welcome to Sekka ! Confirm Your Email";

                    if (await SendGridAPI.Execute(user.Email, user.UserName, txt, link, title))
                    {
                        return StatusCode(StatusCodes.Status200OK);
                    }

                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        private bool UserNameExistes(string userName)
        {
            return _db.Users.Any(x => x.UserName == userName);
        }

        private bool EmailExistes(string email)
        {
            return _db.Users.Any(x => x.Email == email);
        }
        private bool isEmailValid(string email)
        {
            Regex em = new Regex(@"\w+\@\w+.com|\w+\@\w+.net");
            if (em.IsMatch(email))
            {
                return true;
            }
            return false;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("RegisterationConfirm")]
        public async Task<IActionResult> RegisterationConfirm(string ID, string Token)
        {
            if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(Token))
                return NotFound();
            var user = await _manager.FindByIdAsync(ID);

            if (user == null)
                return NotFound();

            
            var newToken = WebEncoders.Base64UrlDecode(Token);
            var encodeToken = Encoding.UTF8.GetString(newToken);


            var result = await _manager.ConfirmEmailAsync(user, encodeToken);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }






        [AllowAnonymous]
        [HttpPost]
        [Route("LoginMobileAgain")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> LoginMobileAgain(LoginModel model)
        {
            await CreateRoles();
            await CreateAdmin();

            var user = await _manager.FindByEmailAsync(model.Email);

            if (!user.EmailConfirmed)
                return Unauthorized("email is not Confirmed");

           // var user = await _manager.FindByEmailAsync(model.Email);
            if (user != null && await _manager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey")), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }







        [AllowAnonymous]
        [HttpPost]
        [Route("LoginMobile")]
        public async Task<IActionResult> LoginMobile([FromBody]LoginModel model)
        {
            await CreateRoles();
            await CreateAdmin();

            var user = await _manager.FindByEmailAsync(model.Email);

            if (!user.EmailConfirmed)
                return Unauthorized("email is not Confirmed");

            if (user != null && await _manager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

                var token = new JwtSecurityToken(
                    issuer: "http://dotnetdetail.net",
                    audience: "http://dotnetdetail.net",
                    expires: DateTime.Now.AddHours(10),
                    claims: authClaims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }





        
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            await CreateRoles();
            await CreateAdmin();

            if (model == null)
                return NotFound();
            var user = await _manager.FindByEmailAsync(model.Email);

            if (user == null)
                return NotFound();

            if (!user.EmailConfirmed)
                return Unauthorized("email is not Confirmed");

            var userName = HttpContext.User.Identity.Name;
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if( id != null && userName != null)
            {
                return BadRequest($"User id:{id} exists");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                //if (await _roleManager.RoleExistsAsync("User"))
                //{
                //    if (!await _manager.IsInRoleAsync(user, "User"))
                //    {
                //        await _manager.AddToRoleAsync(user, "User");
                //    }
                //}

                var roleName = await GetRoleNameByUserId(user.Id);

                if (roleName != null)
                {
                    AddCookies(user.UserName, roleName, user.Id, model.RememberMe, user.Email);
                }
                return Ok();

            }
            else if (result.IsLockedOut)
            {
                return Unauthorized("This Account is locked");
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        private async Task<string> GetRoleNameByUserId(string userId)
        {
            var userRole = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userRole != null)
            {
                return await _db.Roles.Where(x => x.Id == userRole.RoleId).Select(x => x.Name).FirstOrDefaultAsync();
            }
            return null;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }

        private async Task CreateAdmin()
        {
            var admin = await _manager.FindByNameAsync("Admin");
            if (admin == null)
            {
                var user = new ApplicationUser
                {
                    Email = "Admin@admin.com",
                    UserName = "Admin",
                    EmailConfirmed = true
                };

                var x = await _manager.CreateAsync(user, "12345678aA!");

                if (x.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _manager.AddToRoleAsync(user, "Admin");
                    }

                }
            }
        }



        private async Task CreateRoles()
        {
            if (_roleManager.Roles.Count() < 1)
            {
                var role = new ApplicationRole
                {
                    Name = "Admin"
                };
                await _roleManager.CreateAsync(role);

                role = new ApplicationRole
                {
                    Name = "User"
                };
                await _roleManager.CreateAsync(role);
            }
        }


        private async void AddCookies(string userName, string roleName, string UserId, bool remember, string email)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, UserId),
                new Claim(ClaimTypes.Role, roleName),
            };

            var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

            if (remember)
            {
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(10)
                };

                await HttpContext.SignInAsync
                (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity),
                    authProperties
                );
            }
            else
            {
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync
                (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity),
                    authProperties
                );
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetRoleName/{email}")]
        public async Task<string> GetRoleName(string email)
        {
            var user = await _manager.FindByEmailAsync(email);
            if (user != null)
            {
                var userRole = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id);
                if (userRole != null)
                {
                    return await _db.Roles.Where(x => x.Id == userRole.RoleId).Select(x => x.Name).FirstOrDefaultAsync();
                }
            }

            return null;
        }

        [Authorize]
        [HttpGet]
        [Route("CheckUserClaims/{email}&{role}")]
        public IActionResult CheckUserClaims(string email, string role)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userEmail != null && userRole != null && id != null)
            {
                if (email == userEmail && role == userRole)
                {
                   return StatusCode(StatusCodes.Status200OK);
                }
            }

            return StatusCode(StatusCodes.Status203NonAuthoritative);
        }

    }
}
