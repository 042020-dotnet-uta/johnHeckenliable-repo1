
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Models;
using RevatureP1.Web.Helpers;
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
        public ActionResult UserLogin(string email)
        {
            if (ModelState.IsValid)
            {
                var cust = customerRepo.Find(cust => cust.Email == email).Result.FirstOrDefault();

                if (cust != null)
                {
                    CreateClaimIdentity(email);
                    AddUserToSession(cust);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult CreateNew()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNew(Customer user)
        {
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var cust = await customerRepo.Add(user);

                    CreateClaimIdentity(user.Email);
                    AddUserToSession(cust);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }

        private async void CreateClaimIdentity(string email)
        {
            var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, email.StartsWith("admin") ? "Admin" : "Customer")
                }; 
            var identity = new ClaimsIdentity(userClaims, "User Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { identity });
            await HttpContext.SignInAsync(userPrincipal);
        }
        private void AddUserToSession(Customer cust)
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Customer", cust);
        }
    }
}