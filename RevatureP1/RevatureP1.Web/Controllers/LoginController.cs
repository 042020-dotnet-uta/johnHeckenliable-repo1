
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RevatureP1.Domain.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IUnitOfWork unitOfWork,
            ILogger<LoginController> logger) 
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
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
                Customer cust;
                if (email.StartsWith("admin", System.StringComparison.OrdinalIgnoreCase))
                {
                    cust = new Customer
                    {
                        CustomerId = 999,
                        Email = email,
                        FirstName = "Admin",
                        LastName = "Admin"
                    };
                }
                else
                    cust = _unitOfWork.CustomerRepository.Find(cust => cust.Email == email).Result.FirstOrDefault();

                if (cust != null)
                {
                    CreateClaimIdentity(email);
                    AddUserToSession(cust);
                    return RedirectToAction("Index", "Home");
                }
                else
                    _logger.LogDebug("Unable to find a matching email in the db in Login//UserLogin");
            }
            _logger.LogDebug("The model state was invalid in Login//UserLogin");
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
                    var cust = await _unitOfWork.CustomerRepository.Add(user);

                    CreateClaimIdentity(user.Email);
                    AddUserToSession(cust);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }

        /// <summary>
        /// This method will create the appropriate claims identity
        /// to be used by the authentication cookie and to limit/grant access
        /// to the appropriate routes
        /// </summary>
        /// <param name="email">The customers email address.</param>
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
        /// <summary>
        /// The method will add the customer data into the session
        /// </summary>
        /// <param name="cust"></param>
        private void AddUserToSession(Customer cust)
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Customer", cust);
        }
    }
}