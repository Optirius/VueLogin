using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueLogin.BindingModel;
using VueLogin.Data;
using VueLogin.Models;

namespace VueLogin.Controllers
{
    public class AccountController : Controller
    {

        public UserManager<User> usermanager;
        public RoleManager<IdentityRole<Guid>> roleManager;
        public SignInManager<User> signInManager;
        public ApplicationDbContext applicationDbContext;


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
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult UserList()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            User user = await usermanager.FindByEmailAsync(userLogin.Email);
            Microsoft.AspNetCore.Identity.SignInResult result;

            if (ModelState.IsValid && (result = await signInManager.PasswordSignInAsync(user, userLogin.Password, false, false)).Succeeded)
            {
                //return RedirectToAction("Index","Home");
                //return Redirect("~/Home/Index");
                return RedirectToAction("Index", "Home");
            }

            return View("Login");

        }

        [HttpPost]
        public async Task<IActionResult> Register(UserLogin userLogin)
        {
            User user = await usermanager.FindByEmailAsync(userLogin.Email);

            if (user == null)
            {
                user = new User { UserName = userLogin.Email, Email = userLogin.Email };
                var data = usermanager.CreateAsync(user, userLogin.Password).Result;
                data = usermanager.AddToRoleAsync(user, "User").Result;

                await signInManager.PasswordSignInAsync(user, userLogin.Password, false, false);

                //if (ModelState.IsValid && (result = ).Succeeded)
                //{
                //    return RedirectToAction("Index", "Privacy");
                //}
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "User Already Exists! Please Log In.";
                return View("Register");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");

        }

        [HttpGet]
        public IQueryable<User> GetUserList()
        {
            return usermanager.Users;
        }
    }
}
