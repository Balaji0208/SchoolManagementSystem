using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32;
using Newtonsoft.Json;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;
using SchoolManagementSystemWebApp.VM;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace SchoolManagementSystemWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICategoryService _categoryService;
        public AuthController(IAuthService authService, ICategoryService categoryService)
        {
            _authService = authService;
            _categoryService = categoryService;
           
        }

        public async Task<IActionResult> IndexRegister()
        {
            List<RegistrationDTO> list = new();

            var response = await _authService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RegistrationDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString(SD.SeesionToken, model.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                return View(obj);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegistrationViewModel roleMasterVM = new();

            var response = await _categoryService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                roleMasterVM.CategoryList = JsonConvert.DeserializeObject<List<CategoriesDTO>>
                  (Convert.ToString(response.Result)).Select(i => new SelectListItem
                  {
                      Text = i.CategoryName,
                      Value = i.CategoryId.ToString()
                  });
            }

            return View(roleMasterVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel obj)
        {
            if (ModelState.IsValid)
            {

                APIResponse result = await _authService.RegisterAsync<APIResponse>(obj.Registration, HttpContext.Session.GetString(SD.SeesionToken));
            if (result != null && result.IsSuccess)
            {
                    TempData["success"] = "Registered successfully";
                    return RedirectToAction(nameof(IndexRegister));
                }
           
            }
            TempData["error"] = "Error encountered.";
            return View(obj);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SeesionToken, "");
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
