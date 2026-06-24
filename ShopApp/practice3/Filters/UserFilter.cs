using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopDomain.Models;
namespace Shop.App.Filters;
public class UserFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        User? user = context.ActionArguments["user"] as User;

        if (user == null || user.Login != "admin" || user.Id != 1)
        {
            context.Result = new JsonResult(new
            {
                message = "No authorization"
            })
            {
                StatusCode = 401
            };
        }
    }
}