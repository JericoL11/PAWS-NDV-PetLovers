using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.PawsSevices;

namespace PAWS_NDV_PetLovers.Controllers.Login
{
    public class LoginsController : Controller
    {
    
        private readonly PAWS_NDV_PetLoversContext _context;

        public LoginsController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Login()
        {
            await UserAccount();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? userName, string? passWord)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(passWord))
            {
                ModelState.AddModelError("", "Please enter your username and password.");
                return View();
            }

            var hashPassword = HashingService.HashData(passWord);
            var userAccount = await _context.UserAccounts
                                    .FirstOrDefaultAsync(a => a.userName == userName && a.passWord == hashPassword);

            if (userAccount != null)
            {
                // Set authentication session and role
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("UserName", userName); //optional for dashboar display
                HttpContext.Session.SetString("UserRole", userAccount.userType ?? ""); // Store user role
                HttpContext.Session.SetInt32("UserId", userAccount.acc_Id);

                if (!userAccount.IsActive)
                {
                    ModelState.AddModelError("", "Account has been deactivated");
                    return View();
                }

                // Check if the password has been changed
                if (!userAccount.IsPasswordChanged)
                {
                    HttpContext.Session.SetString("PasswordChanged", "false");
                    return RedirectToAction("SignUp", "UserAccounts");
                }
                else
                {
                    HttpContext.Session.SetString("PasswordChanged", "true");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Username or password is incorrect.");
                return View();
            }
        }


        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to the login page
            return RedirectToAction("Login", "Logins");

        }


        //funtion auto craete Admin
        public async Task UserAccount()
        {
            var userAccount = await _context.UserAccounts.ToListAsync
                ();

            if (!(userAccount.Count() > 0))
            {
                const string username = "admin";
                string password = "admin";

                string HashPassword = HashingService.HashData(password);
                password = HashPassword;

                var adminEntry = new UserAccount
                {
                    userName = username,
                    passWord = password,
                    userType = "Admin",
                    IsActive = true,
                    dateCreated = DateTime.Now
                };

                _context.UserAccounts.Add(adminEntry);

                await _context.SaveChangesAsync();
            }
        }
    }
}

