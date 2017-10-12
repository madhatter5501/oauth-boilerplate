using System.Diagnostics;
using Boilerplate.OAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.OAuth.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
