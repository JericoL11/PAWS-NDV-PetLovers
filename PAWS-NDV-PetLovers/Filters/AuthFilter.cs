using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PAWS_NDV_PetLovers.Data;
using Microsoft.EntityFrameworkCore;

public class AuthFilter : ActionFilterAttribute
{
    private readonly PAWS_NDV_PetLoversContext _context;

/*
    // This filter is used for user authentication
    public override async void OnActionExecuting(ActionExecutingContext context)
    {
        // CACHING - Prevent caching of the page
        context.HttpContext.Response.Headers.Add("Cache-Control", "no-store");
        context.HttpContext.Response.Headers.Add("Pragma", "no-cache");
        context.HttpContext.Response.Headers.Add("Expires", "0");

        // Check if the user is authenticated
        var isAuthenticated = context.HttpContext.Session.GetString("IsAuthenticated");
        if (string.IsNullOrEmpty(isAuthenticated))
        {
            // Redirect to login if not authenticated
            context.Result = new RedirectToActionResult("Login", "Logins", null);
            return;
        }


        base.OnActionExecuting(context);
    }*/
}
