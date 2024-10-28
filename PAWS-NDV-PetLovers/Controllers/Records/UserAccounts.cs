using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.PawsSevices;
using PAWS_NDV_PetLovers.ViewModels;

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


        public IActionResult Account(UserAccountVm vm)
        {
            if (vm.activeAccountTab == null)
            {
                vm.activeAccountTab = AccountTab.accountList;
            }
            return View(vm);
        }

        //admin actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageAccount(int userId, string action)
        {
            var userAccount = await _context.UserAccounts.FindAsync(userId);

            if (userAccount != null)
            {
                switch (action)
                {
                    case "Reset":
                        var hashData = HashingService.HashData(userAccount.userName);
                        userAccount.passWord = hashData;
                        userAccount.IsPasswordChanged = false;
                        break;

                    case "Deactivate":
                        userAccount.IsActive = false;
                        break;

                    case "Activate":
                        userAccount.IsActive = true;
                        break;

                    case "Delete":
                        _context.UserAccounts.Remove(userAccount);
                        break;

                    default:
                        // Handle unknown action if necessary
                        break;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Account));
        }

        public IActionResult SwitchToTab(string tabName)
        {
            var vm = new UserAccountVm();

            switch (tabName)
            {
                case "accountList":
                    vm.activeAccountTab = AccountTab.accountList;
                    break;

                case "createStaff":
                    vm.activeAccountTab = AccountTab.createStaff;
                    break;

                default:
                    vm.activeAccountTab = AccountTab.accountList;
                    break;
            }

            return RedirectToAction(nameof(Account), vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStaff(string fname, string lname)
        {
            var vm = new UserAccountVm();

         
            if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname))
            {
                vm.isNull = true;
                vm.activeAccountTab = AccountTab.createStaff;

                return RedirectToAction("Account", vm);
            }

            //name already exist
            var userAccountCheck = await _context.UserAccounts.FirstOrDefaultAsync(u => u.fname == fname && u.lname == lname && u.IsActive == true);

            if (userAccountCheck != null)
            {
                vm.isExist = true;
                vm.activeAccountTab = AccountTab.createStaff;

                return RedirectToAction("Account", vm);
            }

            var username_Passsowrd = $"ndv.{fname}{lname}";

            var hashData = HashingService.HashData(username_Passsowrd);

           //capitalizing first letter
           var Fname = char.ToUpper(fname[0]) +fname.Substring(1);
            var Lname = char.ToUpper(lname[0]) + lname.Substring(1);

            if (ModelState.IsValid)
            {
                var userAccount = new UserAccount
                {
                    fname = Fname,
                    lname = Lname,
                    userName = username_Passsowrd,
                    passWord = hashData,
                    IsActive = true,
                    dateCreated = DateTime.Now,
                    userType = "Staff"
                };

                _context.UserAccounts.Add(userAccount);
                await _context.SaveChangesAsync();
            }

            vm.activeAccountTab = AccountTab.accountList;
         
            return RedirectToAction("Account", vm );
        }

        public async Task<IActionResult> SignUp()
        {

            //get id from login session
            var id = HttpContext.Session.GetInt32("UserId");

            var staffAcc = await _context.UserAccounts.FirstOrDefaultAsync(u => u.acc_Id == id);


            if (!(string.IsNullOrEmpty(staffAcc.fname) && string.IsNullOrEmpty(staffAcc.lname)))
            {
             
                return View(new UserAccountVm
                {
                    userAccount = staffAcc
                });
            }

            if ((string.IsNullOrEmpty(staffAcc.email)
                && string.IsNullOrEmpty(staffAcc.contact)
                && string.IsNullOrEmpty(staffAcc.bdate.ToString())) 
                && staffAcc.IsPasswordChanged == false)
            {
                return View(new UserAccountVm
                {
                    fromReset = true,
                });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> SignUp(string? passWord, string? confirmpassWord, UserAccountVm userAcctVm)
        {
            var userAcct = userAcctVm.userAccount;

            //get id from login session
            var id =  HttpContext.Session.GetInt32("UserId");

            //conditions
            if (string.IsNullOrWhiteSpace(passWord) || string.IsNullOrWhiteSpace(confirmpassWord))
            {
                ModelState.AddModelError("", "Please input Password");
                return View( new UserAccountVm{
                    userAccount = userAcct
                });
            }

            if (passWord.Count() <= 8)
            {
                ModelState.AddModelError("","Password below 8 characters is invalid");
                return View(new UserAccountVm
                {
                    userAccount = userAcct
                });
            }

            if(passWord != confirmpassWord)
            {
                ModelState.AddModelError("", "Password doesn't match");
                return View(new UserAccountVm
                {
                    userAccount = userAcct
                });
            }

            //check
            var userAccount = await _context.UserAccounts.FirstOrDefaultAsync(u => u.acc_Id == id);

            //if found
            if (userAccount != null)
            {

                var hashing = HashingService.HashData(passWord);

                //updateUser
                userAccount.fname = userAcct.fname;
                userAccount.lname = userAcct.lname;
                userAccount.mname = userAcct.mname;
                userAccount.contact = userAcct.contact;
                userAccount.email = userAcct.email;
                userAccount.passWord = hashing;
                userAccount.IsPasswordChanged = true;
                userAccount.bdate = userAcct.bdate;

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
