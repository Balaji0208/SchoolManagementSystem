using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32;
using Newtonsoft.Json;
using SchoolManagementSystemWebApp.AuthService;
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
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;


        public AuthController(IAuthService authService, ICategoryService categoryService,ICountryService countryService,IStateService stateService)
        {
            _authService = authService;
            _categoryService = categoryService;
            _countryService = countryService;
            _stateService = stateService;
           

        }
        [Authorize(Roles = "Register")]
        public async Task<IActionResult> IndexRegister(int currentPage = 1,string orederBy="",string term="")
        {
            RegisterPaginationVM registerVM = new RegisterPaginationVM(); 
            List<RegistrationDTO> list = new();

            var response = await _authService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RegistrationDTO>>(Convert.ToString(response.Result));
            }
            int totalRecords = list.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            registerVM.Register = list;
            registerVM.CurrentPage = currentPage;
            registerVM.PageSize = pageSize;
            registerVM.TotalPages = totalPages;
            registerVM.OrderBy = orederBy;
            return View(registerVM);
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
        [Authorize(Roles = "Register")]
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegistrationViewModel RegistrationMasterVM = new();

            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (response != null && response.IsSuccess)
            {
                RegistrationMasterVM.CategoryList = JsonConvert.DeserializeObject<List<CategoriesDTO>>
                  (Convert.ToString(response.Result)).Select(i => new SelectListItem
                  {
                      Text = i.CategoryName,
                      Value = i.CategoryId.ToString()
                  });
            }
            var Country = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (Country != null && Country.IsSuccess)
            {
                RegistrationMasterVM.CountryList = JsonConvert.DeserializeObject<List<CountryMasterDTO>>
                  (Convert.ToString(Country.Result)).Select(i => new SelectListItem
                  {
                      Text = i.CountryName,
                      Value = i.CountryId.ToString()
                  });
            }

            return View(RegistrationMasterVM);
        }

        public async Task<JsonResult> GetUserDetails(int id) 
        { 
            var response = await _authService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SeesionToken)); 
            if (response != null) 
            { RegistrationDTO user = JsonConvert.DeserializeObject<RegistrationDTO>(Convert.ToString(response.Result)); 
                return Json(user);
            } 
            return Json(null);
        }


        public async Task<JsonResult> GetStateByCountryId(int countryId) 
        { 
            var response = await _stateService.GetStateAsync<APIResponse>(countryId,HttpContext.Session.GetString(SD.SeesionToken)); 
            if (response != null) 
            { List<StateMasterDTO> states = JsonConvert.DeserializeObject<List<StateMasterDTO>>(Convert.ToString(response.Result)); 
                return Json(states); 
            } 
            return Json(null); 
        }
        public async Task<JsonResult> Search(string Prefix)
        {

            var response = await _authService.GetUserByPrefix<APIResponse>(Prefix, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null)
            {
                List<RegistrationDTO> name = JsonConvert.DeserializeObject<List<RegistrationDTO>>(Convert.ToString(response.Result));
                return Json(name);
            }
            return Json(null);

        }
        [Authorize(Roles = "Register")]
        public async Task<IActionResult> EnableRegistration(int registrationId)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.RecoverAsync<APIResponse>(registrationId, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Enabled successfully";
                    return RedirectToAction(nameof(IndexRegister));
                }

                TempData["error"] = "error";
                return RedirectToAction(nameof(IndexRegister));
            }

            return View();
        }


        [Authorize(Roles = "Register")]
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


        [Authorize(Roles = "Register")]
        [HttpGet]
        public async Task<IActionResult> UpdateRegister(int regId)
        {
            RegistrationViewModel RegistrationMasterVM = new();
            var register = await _authService.GetAsync<APIResponse>(regId, HttpContext.Session.GetString(SD.SeesionToken));
            if (register != null && register.IsSuccess)
            {

                RegistrationDTO model = JsonConvert.DeserializeObject<RegistrationDTO>(Convert.ToString(register.Result));
                RegistrationMasterVM.Registration = model;
            }

            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (response != null && response.IsSuccess)
            {
                RegistrationMasterVM.CategoryList = JsonConvert.DeserializeObject<List<CategoriesDTO>>
                  (Convert.ToString(response.Result)).Select(i => new SelectListItem
                  {
                      Text = i.CategoryName,
                      Value = i.CategoryId.ToString()
                  });
            }
            var Country = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (Country != null && Country.IsSuccess)
            {
                RegistrationMasterVM.CountryList = JsonConvert.DeserializeObject<List<CountryMasterDTO>>
                  (Convert.ToString(Country.Result)).Select(i => new SelectListItem
                  {
                      Text = i.CountryName,
                      Value = i.CountryId.ToString()
                  });
            }
            var State = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));


            if (State != null && State.IsSuccess)
            {
                RegistrationMasterVM.StateList = JsonConvert.DeserializeObject<List<StateMasterDTO>>
                  (Convert.ToString(State.Result)).Select(i => new SelectListItem
                  {
                      Text = i.StateName,
                      Value = i.StateId.ToString()
                  });
            }




            return View(RegistrationMasterVM);
        }
            
        
        [Authorize(Roles = "Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRegister(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                APIResponse result = await _authService.UpdateAsync<APIResponse>(model.Registration, HttpContext.Session.GetString(SD.SeesionToken));
                if (result != null && result.IsSuccess)
                {
                    TempData["success"] = "Registered successfully";
                    return RedirectToAction(nameof(IndexRegister));
                }

            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }



       
        public async Task<IActionResult> DeleteRegister(int regId)
        {

            var response = await _authService.DeleteAsync<APIResponse>(regId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Register deleted successfully";
                return RedirectToAction(nameof(IndexRegister));
            }
            TempData["error"] = "Error encountered.";
            return View();
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
