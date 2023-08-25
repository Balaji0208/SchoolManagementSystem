using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SchoolManagementSystemWebApp.Controllers
{
    public class UserController : Controller

    {
        private readonly IAuthService _authService;
        private readonly IUserLoginService _userLoginService;
        public UserController(IAuthService authService, IUserLoginService userLoginService)
        {
            _authService = authService;
            _userLoginService = userLoginService;

        }

        public async Task<IActionResult> IndexUserLogin()
        {
            List<UserDTO> list = new();

            var response = await _userLoginService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> UserLogin(UserDTO obj)
        {
            APIResponse response = await _userLoginService.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                return View(obj);
            }
        }
    }
}
