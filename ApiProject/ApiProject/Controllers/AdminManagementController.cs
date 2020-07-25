using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiProject.Models;
using ApiProject.Modelviews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;




namespace ApiProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminManagementController : ControllerBase

    {
        /*
         1=> h4of h2led elconstructor bta3 elaccount 
         2=> fi link 3ltelegram da kan fi function lock out 

              */


        private readonly ApplicationDb _Context;
        private readonly UserManager<ApplicationUser> _manager;
        // private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminManagementController(ApplicationDb Context, UserManager<ApplicationUser> manager/*,SignInManager<ApplicationUser> signInManager*//* */, RoleManager<ApplicationRole> roleManager)
        {
            _Context = Context;
            _manager = manager;
            // _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("ListUsers")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> ListUsers()
        {
            return await _Context.ApplicationUser.ToListAsync();
            

        }

        // GET: api/Users/5
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult<ApplicationUser>> Details(int id)
        {
            var user = await _Context.ApplicationUser.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //[HttpPost]
        //[Route("AddUer")]
        //public async Task<ActionResult<ApplicationUser>> AddUer(ApplicationUser applicationUser)
        //{
        //    _context.ApplicationUser.Add(applicationUser);
        //    await _context.SaveChangesAsync();

        //    return Ok("Added Successfully");
        //}

        // [HttpPost]
        //[ValidateAntiForgeryToken]
        // public JsonResult Update([Bind(Exclude = null)] StudentViewModel model)
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(string id, EditViewModel model)
        {
            /*    if (ModelState.IsValid)
                {
                    ApplicationUser user = UserManager.FindById(model.id);

                    user = new user
                    {
                        Name = model.UserName,

                        Email = model.Email,

                        //PasswordHash = checkUser.PasswordHash
                    };

                    UserManager.Update(user);
                }*/

            // Get the existing student from the db
            // var user = (Users)ApplicationUser.FindById(model.id);
            var user = await _manager.FindByIdAsync(id);

            // Update it with the values from the view model
            user.UserName = model.UserName;

            user.Email = model.Email;


            //user.PasswordHash = model.Password;

            // Apply the changes if any to the db
            _manager.UpdateAsync(user);
            // _context.Entry(user).State = EntityState.Modified;
            return Ok("done");

        }



        //TO ADD USER
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(RegisterModel model)
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

                    return Ok("DONE YA M3LM");
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
            return _Context.Users.Any(x => x.UserName == userName);
        }

        private bool EmailExistes(string email)
        {
            return _Context.Users.Any(x => x.Email == email);
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


        //TO BLOCK USER
        [HttpPut]
        [Route("BLock")]
        public async Task<ActionResult<ApplicationUser>> BLockUser(BlockUser model)
        {
            var user = await _manager.FindByNameAsync(model.UserName);

            if (!model.IsLocked)
            {
                await _manager.SetLockoutEnabledAsync(user, false);
                return Ok("Account is Locked");
            }
            else
            {
                await _manager.SetLockoutEnabledAsync(user, true);
                return Ok("Account is Unocked ");
            }
        }




        //await _userManager.SetLockoutEnabledAsync(user, true);


        //TO DELETE USER
        // DELETE: api/Users/5
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<ApplicationUser>> DeleteUser(string id)
        {
            var applicationUser = await _Context.ApplicationUser.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _Context.ApplicationUser.Remove(applicationUser);
            await _Context.SaveChangesAsync();

            return Ok("E4ta 3lik ");
        }

        private bool ApplicationRoleExists(string id)
        {
            return _Context.Roles.Any(e => e.Id == id);
        }
    }



}
