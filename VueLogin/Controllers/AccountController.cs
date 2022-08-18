using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueLogin.BindingModel;
using VueLogin.Models;

namespace VueLogin.Controllers
{
    public class AccountController : Controller
    {

        public UserManager<User> usermanager;
        public RoleManager<IdentityRole<Guid>> roleManager;
        public SignInManager<User> signInManager; 


        public AccountController(UserManager<User> usermanager, RoleManager<IdentityRole<Guid>> roleManager, SignInManager<User> signInManager)
        {
            this.usermanager = usermanager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

            if (!roleManager.Roles.Any())
            {
                var result = roleManager.CreateAsync(new IdentityRole<Guid> {Id= Guid.NewGuid(), Name= "Admin" }).Result;
                result = roleManager.CreateAsync(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User" }).Result;

                var user = new User { UserName = "admin@abc.com", Email = "admin@abc.com" };
                result = usermanager.CreateAsync(user, "Admin@123").Result;
                result = usermanager.AddToRoleAsync(user, "Admin").Result;

                user = new User { UserName = "user@abc.com", Email = "user@abc.com" };
                result = usermanager.CreateAsync(user, "User@123").Result;
                result = usermanager.AddToRoleAsync(user, "User").Result;

            }


        }


        public IActionResult Login()
        {
            return View("Login");
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserLogin userLogin)
        {
            User user = await usermanager.FindByEmailAsync(userLogin.Email);
            Microsoft.AspNetCore.Identity.SignInResult result;

            if (ModelState.IsValid && (result = await signInManager.PasswordSignInAsync(user, userLogin.Password, false, false)).Succeeded)
            {
                return View("~/Home/Index");
            }

            return View("Login");

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserLogin userLogin)
        {
            User user = await usermanager.FindByEmailAsync(userLogin.Email);

            if (user == null)
            {
                user = new User { UserName = userLogin.Email, Email = userLogin.Email };
                var data = usermanager.CreateAsync(user, userLogin.Password).Result;
                data = usermanager.AddToRoleAsync(user, "User").Result;

                Microsoft.AspNetCore.Identity.SignInResult result;

                await signInManager.PasswordSignInAsync(user, userLogin.Password, false, false);

                //if (ModelState.IsValid && (result = ).Succeeded)
                //{
                //    return RedirectToAction("Index", "Privacy");
                //}

                return View("~/Home/Index");
            }
            else
            {
                Message message = new Message();
                message.Text = "User Already Exists. Please Login.";

                return View("Register", message);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");

        }
    }
}
