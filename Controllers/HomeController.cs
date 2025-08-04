using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;;
        }

        [AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
        public async Task<IActionResult> Index()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["GraphApiResult"] = user.DisplayName;
            return View();
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> PrivatePage()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["name"] = user.DisplayName;
            ViewData["oid"] = user.Id;
            ViewData["email"] = user.Mail ?? user.UserPrincipalName;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
