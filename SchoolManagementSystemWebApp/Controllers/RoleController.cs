using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;
using System.Data;

namespace SchoolManagementSystemWebApp.Controllers
{
    public class RoleController : Controller
    {
       
            private readonly IRoleService _roleService;
           

           
            public RoleController(IRoleService roleService)
            {
                _roleService = roleService;
               
            }

            public async Task<IActionResult> IndexRole()
            {
                List<RoleDetailsDTO> list = new();
                var response = await _roleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<RoleDetailsDTO>>(Convert.ToString(response.Result));
                }
                return View(list);
            }
        [Authorize(Roles = "Register")]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleDetailsDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _roleService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(IndexRole));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Register")]
        public async Task<IActionResult> UpdateRole(int roleId)
        {
            var response = await _roleService.GetAsync<APIResponse>(roleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {

                RoleDetailsDTO model = JsonConvert.DeserializeObject<RoleDetailsDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(RoleDetailsDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Admin updated successfully";
                var response = await _roleService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexRole));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
       
        
        public async Task<IActionResult> DeleteRole(int roleId)
        {

            var response = await _roleService.DeleteAsync<APIResponse>(roleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Admin deleted successfully";
                return RedirectToAction(nameof(IndexRole));
            }
            TempData["error"] = "Error encountered.";
            return View();
        }


    }
}
       
