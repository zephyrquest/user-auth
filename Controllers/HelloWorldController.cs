using Microsoft.AspNetCore.Mvc;

namespace user_auth.Controllers
{
    public class HelloWorldController : Controller
    {
        // 
        // GET: /HelloWorld/ 

        public IActionResult Index()
        {
            return View();
        }

        // 
        // GET: /HelloWorld/Welcome/{name}/{id}  route defined in Program.cs 

        public IActionResult Welcome(string name, int id = 1)
        {
            ViewData["name"] = name;
            ViewData["id"] = id;
            return View();
        }

        // route defined explicity here
        [Route("HelloWorld/Planet/{name}")]
        public IActionResult Planet(string name)
        {
            ViewData["name"] = name;
            return View();
        }
    }
}
