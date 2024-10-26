using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.PawsSevices;

namespace PAWS_NDV_PetLovers.Controllers.Records
{
    [ServiceFilter(typeof(AuthFilter))]
    public class UserAccounts : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public UserAccounts(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Account(string? email,string? passWord, string? confirmpassWord)
        {
            //get id from login session
            var id =  HttpContext.Session.GetInt32("UserId");

            //conditions
            if (string.IsNullOrWhiteSpace(passWord) || string.IsNullOrWhiteSpace(confirmpassWord))
            {
                ModelState.AddModelError("", "Please input Password");
                return View();
            }

            if (passWord.Count() <= 8)
            {
                ModelState.AddModelError("","Password below 8 characters is invalid");
                return View();
            }

            if(passWord != confirmpassWord)
            {
                ModelState.AddModelError("", "Password doesn't match");
                return View();
            }

            //check
            var userAccount = await _context.UserAccounts.FirstOrDefaultAsync(u => u.acc_Id == id);

            //if found
            if (userAccount != null)
            {

                var hashing = HashingService.HashData(passWord);

                //updateUser
                userAccount.email = email;
                userAccount.passWord = hashing;
                userAccount.IsPasswordChanged = true;

                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("PasswordChanged", "true");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return NotFound();
            }
         
        }
    }
}
