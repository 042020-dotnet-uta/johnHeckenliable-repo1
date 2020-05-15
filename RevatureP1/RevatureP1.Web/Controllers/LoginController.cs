
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RevatureP1.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IRepository<Customer> customerRepo;
        public LoginController(IRepository<Customer> customerRepo) 
        {
            this.customerRepo = customerRepo;
        }

        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin([Bind]string email)
        {
            var cust = customerRepo.Find(cust => cust.Email == email).Result.FirstOrDefault();

            if (cust != null)
            {
                var userClaims = new List<Claim>()
                {
                    new Claim("Id", cust.CustomerId.ToString()),
                    new Claim(ClaimTypes.Role, cust.Email.StartsWith("admin") ? "Admin" : "Customer")
                };

                var identity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { identity });
                HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateNew()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateNew([Bind] Customer user)
        {

            if (user != null)
            {
                var cust = await customerRepo.Add(user);

                var userClaims = new List<Claim>()
                {
                    new Claim("Id", cust.CustomerId.ToString()),
                    new Claim(ClaimTypes.Role, user.Email.StartsWith("admin") ? "Admin" : "Customer")
                };

                var identity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { identity });
                await HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }
    }
}