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

            // Redirect to the home page or another authenticated area
            return RedirectToAction("Index", "Home");

            #region -- temp comments --
            /*var hashPassword = HashingService.HashData(passWord);

            var userAccount = await _context.UserAccounts.FirstOrDefaultAsync(a => a.userName == userName && a.passWord == hashPassword);


            if (userAccount != null)
            {
                // If authentication is successful:
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString($"UserName", $"{userName}");

                // Set a cookie to indicate the user is logged in
                Response.Cookies.Append("UserAuthCookie", "true");

                // Redirect to the home page or another authenticated area
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("", "Username or Password is Incorrect.");
                return View();
            }*/
            #endregion
        }

        public IActionResult Logout()
        {


            #region -- temp comments -- 
            // Clear session data
           /* HttpContext.Session.Clear();
            // Remove authentication cookie
            Response.Cookies.Delete("UserAuthCookie");

            // Optionally, add headers to prevent caching
            Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");*/
            #endregion
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
                    userType = "Admin"
                };

                _context.UserAccounts.Add(adminEntry);

                await _context.SaveChangesAsync();
            }
        }
    }
}

