﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task5.ViewModels; 
using Task5.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Task5.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    if (user.Status == false)
                    {
                        ModelState.AddModelError("", "Ваш статус Block");
                    }
                    else
                    {
                        user.LastLoginDate = model.LastLoginDate;
                        await db.SaveChangesAsync();

                        await Authenticate(model.Email);

                        return RedirectToAction("Index", "Home");
                    }  
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    db.Users.Add(new User { Name = model.Name, Email = model.Email, Password = model.Password, LastLoginDate = model.LastLoginDate });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int[] items)
        {

            if (items != null)
            {
                foreach (var item in items)
                {
                    User user = await db.Users.FirstOrDefaultAsync(p => p.Id == item);
                    if (user != null)
                    {
                        db.Users.Remove(user);
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index", "Home");

            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Block(int[] items)
        {

            if (items != null)
            {
                foreach (var item in items)
                {
                    User user = await db.Users.FirstOrDefaultAsync(p => p.Id == item);
                    if (user != null)
                    {
                        
                        user.Status = false;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index", "Home");

            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(int[] items)
        {

            if (items != null)
            {
                foreach (var item in items)
                {
                    User user = await db.Users.FirstOrDefaultAsync(p => p.Id == item);
                    if (user != null)
                    {
                        user.Status = true;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index", "Home");

            }
            return NotFound();
        }
    }
}
