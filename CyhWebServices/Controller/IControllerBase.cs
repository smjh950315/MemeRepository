using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using System.Text;
namespace Cyh.WebServices.Controller
{
    public partial interface IControllerBase
    {
        public HttpContext HttpContext { get; }

        public HttpRequest Request { get; }

        public HttpResponse Response { get; }

        public RouteData RouteData { get; }

        public ModelStateDictionary ModelState { get; }

        public ClaimsPrincipal User { get; }

        [NonAction]
        public ContentResult Content(string content);

        [NonAction]
        public ContentResult Content(string content, string contentType);

        [NonAction]
        public ContentResult Content(string content, string contentType, Encoding contentEncoding);

        [NonAction]
        public NoContentResult NoContent();

        [NonAction]
        public OkResult Ok();

        [NonAction]
        public RedirectToActionResult RedirectToAction();

        [NonAction]
        public RedirectToActionResult RedirectToAction(string? actionName);

        [NonAction]
        public RedirectToActionResult RedirectToAction(string? actionName, object? routeValues);

        [NonAction]
        public RedirectToActionResult RedirectToAction(string? actionName, string? controllerName, object? routeValues);


        [NonAction]
        public RedirectToRouteResult RedirectToRoute(string? routeName);

        [NonAction]
        public RedirectToRouteResult RedirectToRoute(object? routeValues);

        [NonAction]
        public RedirectToRouteResult RedirectToRoute(string? routeName, object? routeValues);

    }
}
