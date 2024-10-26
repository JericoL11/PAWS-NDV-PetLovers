using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PAWS_NDV_PetLovers.Data;
using System.Collections.Generic;
using System.Linq;

public class AuthFilter : ActionFilterAttribute
{
    private readonly PAWS_NDV_PetLoversContext _context;

    public AuthFilter(PAWS_NDV_PetLoversContext context)
    {
        _context = context;
    }

    public override async void OnActionExecuting(ActionExecutingContext context)
    {
        // Prevent caching of sensitive pages
        context.HttpContext.Response.Headers.Add("Cache-Control", "no-store");
        context.HttpContext.Response.Headers.Add("Pragma", "no-cache");
        context.HttpContext.Response.Headers.Add("Expires", "0");

        // Check if user is authenticated
        var isAuthenticated = context.HttpContext.Session.GetString("IsAuthenticated");
        if (string.IsNullOrEmpty(isAuthenticated))
        {
            // Redirect to login if not authenticated
            context.Result = new RedirectToActionResult("Login", "Logins", null);
            return;
        }

        // Retrieve user role and password status from session
        var passwordChanged = context.HttpContext.Session.GetString("PasswordChanged");
        var userRole = context.HttpContext.Session.GetString("UserRole");

        // Redirect to login if role is missing (incomplete account or error)
        if (string.IsNullOrEmpty(userRole))
        {
            context.Result = new RedirectToActionResult("Login", "Logins", null);
            return;
        }

        // Allow only access to the Edit Account page if the password hasn't been changed
        var isEditAccountPage = context.RouteData.Values["action"]?.ToString() == "Account" &&
                                context.RouteData.Values["controller"]?.ToString() == "UserAccounts";
        if (passwordChanged == "false" && !isEditAccountPage)
        {
            context.Result = new RedirectToActionResult("Account", "UserAccounts", null);
            return;
        }

        // Admins have unrestricted access
        if (userRole == "admin")
        {
            base.OnActionExecuting(context);
            return;
        }

        // Define restricted actions for staff role
        var restrictedActionsForStaff = new List<(string controller, string action)>
        {
            ("Admin", "Dashboard"),   // Restrict access to Admin Dashboard for staff
            ("Reports", "Financial")  // Restrict access to Financial Reports for staff
        };

        // Restrict access for staff if trying to access restricted actions
        if (userRole == "staff")
        {
            var currentController = context.RouteData.Values["controller"]?.ToString();
            var currentAction = context.RouteData.Values["action"]?.ToString();

            if (restrictedActionsForStaff.Any(r => r.controller == currentController && r.action == currentAction))
            {
                // Redirect staff user to Access Denied page
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                return;
            }
        }

        // Continue if all checks pass
        base.OnActionExecuting(context);
    }
}
