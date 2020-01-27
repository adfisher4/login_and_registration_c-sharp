using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginAndRegistration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LoginAndRegistration.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Registering(User newUser)
        {      
        if(ModelState.IsValid)
            {
            // If a User exists with provided email
            if(dbContext.Users.Any(u => u.Email == newUser.Email))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Register");
                
                // You may consider returning to the View at this point
            }
            else {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Add(newUser);
                dbContext.SaveChanges();
                return RedirectToAction("Login");
            }
        }
        return View("Register");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            string UserEmail = HttpContext.Session.GetString("Email");
        if (UserEmail != null) {
            return RedirectToAction ("Success");
        }
        else {
            return View();
        }
        }

        [HttpPost("attempt")]
        public IActionResult LoginAttempt(LoginUser userSubmission)
    {
        
        if(ModelState.IsValid)
        {
            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
            // If no user exists with provided email
            if(userInDb == null || userSubmission.Password == null)
            {
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Login");
            }

            var hasher = new PasswordHasher<LoginUser>();
            
            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
            
            // result can be compared to 0 for failure
            if(result == 0)
            {
                // handle failure (this should be similar to how "existing email" is handled)
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Login");
            }
        }
        HttpContext.Session.SetString("Email", userSubmission.Email);
        string UserEmail = HttpContext.Session.GetString("Email");

        return RedirectToAction ("Success");
    }

        [HttpGet("success")]
        public IActionResult Success()
        {
            string UserEmail = HttpContext.Session.GetString("Email");
        if (UserEmail != null) {
            return View();
        }
        else {
                return RedirectToAction("Login"); 
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout ()
        {

            HttpContext.Session.Clear();
            Console.WriteLine("Clearing session...");
            return RedirectToAction("Register");
        }  

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
