using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolManagementSystemWebApp.AuthService;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;
using SchoolManagementSystemWebApp.VM;
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> IndexRole(int currentPage = 1, string orederBy = "", string term = "")
            {
            RoleVM roleVM = new RoleVM();

            IEnumerable<RoleDetailsDTO> list = new List<RoleDetailsDTO>();

            var response = await _roleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RoleDetailsDTO>>(Convert.ToString(response.Result));
            }

            int totalRecords = list.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            roleVM.Role = list;
            roleVM.CurrentPage = currentPage;
            roleVM.PageSize = pageSize;
            roleVM.TotalPages = totalPages;
            roleVM.OrderBy = orederBy;
            return View(roleVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> EnableRole(int roleId)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleService.RecoverAsync<APIResponse>(roleId, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Enabled successfully";
                    return RedirectToAction(nameof(IndexRole));
                }

                TempData["error"] = "error";
                return RedirectToAction(nameof(IndexRole));
            }

            return View();
        }




    }
}
       
