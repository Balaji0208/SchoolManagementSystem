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
using SchoolManagementSystemWebApp.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using SchoolManagementSystemWebApp.AuthService;
using System.Text.RegularExpressions;

namespace SchoolManagementSystemWebApp.Controllers
{
    public class UserController : Controller

    {
        private readonly IAuthService _authService;
        private readonly IRoleService _roleService;
        private readonly IUserLoginService _userLoginService;

        public UserController(IAuthService authService, IUserLoginService userLoginService, IRoleService roleService)
        {
            _authService = authService;
            _userLoginService = userLoginService;
            _roleService = roleService;
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
        public async Task<IActionResult> UserLoginRegister()
        {
            UserRegistrationViewModel UserRegistrationVM = new();
            var response = await _roleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (response != null && response.IsSuccess)
            {
                UserRegistrationVM.RoleList = JsonConvert.DeserializeObject<List<RoleDetailsDTO>>
                  (Convert.ToString(response.Result)).Select(i => new SelectListItem
                  {
                      Text = i.RoleName,
                      Value = i.RoleId.ToString()
                  });
            }
            var name = await _authService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (name != null && name.IsSuccess)
            {
                UserRegistrationVM.RegisterList = JsonConvert.DeserializeObject<List<RegistrationDTO>>
                  (Convert.ToString(name.Result)).Select(i => new SelectListItem
                  {
                      Text = i.FirstName,
                      Value = i.registerId.ToString()
                  });
            }

            return View(UserRegistrationVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLoginRegister(UserRegistrationViewModel obj)
        {
            if (ModelState.IsValid)
            {

                APIResponse result = await _userLoginService.RegisterAsync<APIResponse>(obj.UserRegistration, HttpContext.Session.GetString(SD.SeesionToken));
                if (result != null && result.IsSuccess)
                {
                    TempData["success"] = "Registered successfully";
                    return RedirectToAction(nameof(IndexUserLogin));
                }

            }
            TempData["error"] = "Error encountered.";
            return View(obj);
        }
        [Authorize(Roles = "Register")]
        public async Task<IActionResult> UpdateUser(int userId)
        {
            UserRegistrationViewModel UserVM = new();
            var register = await _userLoginService.GetAsync<APIResponse>(userId, HttpContext.Session.GetString(SD.SeesionToken));
            if (register != null && register.IsSuccess)
            {

                UserDTO model = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(register.Result));
                UserVM.UserRegistration = model;
            }

            var response = await _roleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (response != null && response.IsSuccess)
            {
                UserVM.RoleList = JsonConvert.DeserializeObject<List<RoleDetailsDTO>>
                  (Convert.ToString(response.Result)).Select(i => new SelectListItem
                  {
                      Text = i.RoleName,
                      Value = i.RoleId.ToString()
                  });
            }
            
            var registerName = await _authService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (registerName != null && registerName.IsSuccess)
            {
                UserVM.RegisterList = JsonConvert.DeserializeObject<List<RegistrationDTO>>
                  (Convert.ToString(registerName.Result)).Select(i => new SelectListItem
                  {
                      Text = i.FirstName,
                      Value = i.registerId.ToString()
                  });
            }
            return View(UserVM);
        }
        [Authorize(Roles = "Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                APIResponse result = await _userLoginService.UpdateAsync<APIResponse>(model.UserRegistration, HttpContext.Session.GetString(SD.SeesionToken));
                if (result != null && result.IsSuccess)
                {
                    TempData["success"] = "Updated successfully";
                    return RedirectToAction(nameof(IndexUserLogin));
                }

            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
     
      
        public async Task<IActionResult> DeleteUser(int userId)
        {

            var response = await _userLoginService.DeleteAsync<APIResponse>(userId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "User deleted successfully";
                return RedirectToAction(nameof(IndexUserLogin));
            }
            TempData["error"] = "Error encountered.";
            return View(userId);
        }
      
    }

        

    }





