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
using System.Threading.Tasks.Dataflow;

namespace SchoolManagementSystemWebApp.Controllers
{
    public class ModuleMappingController : Controller

    {
        private readonly IModuleService _moduleService;
        private readonly IRoleService _roleService;
        private readonly IModuleRoleMappingService _moduleRoleMappingService;

        public ModuleMappingController(IModuleService moduleService, IModuleRoleMappingService moduleRoleMappingService, IRoleService roleService)
        {
            _moduleService = moduleService;
            _moduleRoleMappingService = moduleRoleMappingService;
            _roleService = roleService;
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> IndexModuleMapping(int currentPage = 1, string orederBy = "", string term = "")
        {
            ModuleMappingPaginationVM moduleMappingVm = new ModuleMappingPaginationVM();

            IEnumerable<ModuleRoleMappingDTO> list = new List<ModuleRoleMappingDTO>();

            var response = await _moduleRoleMappingService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ModuleRoleMappingDTO>>(Convert.ToString(response.Result));
            }

            int totalRecords = list.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            moduleMappingVm.ModuleMappingPageination = list;
            moduleMappingVm.CurrentPage = currentPage;
            moduleMappingVm.PageSize = pageSize;
            moduleMappingVm.TotalPages = totalPages;
            moduleMappingVm.OrderBy = orederBy;
            return View(moduleMappingVm);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateModuleMapping()
        {

            ModuleMappinVM roleMenu = new();

            var response = await _roleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null)
            {
                roleMenu.RoleList = JsonConvert.DeserializeObject<List<RoleDetails>>(Convert.ToString(response.Result)).Select(i => new SelectListItem
                {
                    Text = i.RoleName,
                    Value = i.RoleId.ToString(),
                });
            }


            return View(roleMenu);
        }
        [HttpGet]
        public async Task<IActionResult> GetMenusByRole(int roleId)
        {
            var result = await _moduleRoleMappingService.GetMenuAsync<APIResponse>(roleId, HttpContext.Session.GetString(SD.SeesionToken));
            List<ModuleDTO> menus = JsonConvert.DeserializeObject<List<ModuleDTO>>(Convert.ToString(result.Result));
            ModuleMappinVM roleMenu = new();
            // Create a list of CustomSelectListItem as shown in the previous answer

            var menu = await _moduleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (menu != null)
            {
                roleMenu.MenuList = JsonConvert.DeserializeObject<List<ModuleDTO>>(Convert.ToString(menu.Result)).Select(i => new CustomSelectedItem
                {
                    Text = i.Menus,
                    Value = i.ModuleId.ToString(),
                    Selected = false,
                    ParentId = i.ParentId ?? 0,
                    ParentName=i.ParentName
                    
                }).ToList();
                foreach (var item in roleMenu.MenuList)
                {
                    foreach (var men in menus)
                    {
                        if (men.ModuleId.ToString() ==item.Value)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            List<CustomSelectedItem> roleMenulist = new();
            foreach (var item in roleMenu.MenuList)
            {
                if (item.ParentId == 0)
                {
                    roleMenulist.Add(item);
                }
            }
            foreach (var item in roleMenu.MenuList)
            {
                if (item.ParentId == 0)
                {
                    foreach (var subitem in roleMenu.MenuList)
                    {
                        if (subitem.ParentId.ToString() == item.Value)
                        {
                            roleMenulist.Add(subitem);
                        }

                    }
                }
            }
            return Json(roleMenulist); ; // Return the menus as JSON
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModuleMapping(ModuleMappinVM obj, string selectedMenuData)
        {
            try
            {
                List<ModuleRoleMappingDTO> list = new();
                List<CustomSelectedItem> selectedMenusList = JsonConvert.DeserializeObject<List<CustomSelectedItem>>(selectedMenuData);
                var response = await _moduleRoleMappingService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ModuleRoleMappingDTO>>(Convert.ToString(response.Result));

                }
                if (ModelState.IsValid)
                {
                    foreach (var selectedItem in selectedMenusList)
                    {

                        var matchingItem = list.FirstOrDefault(item =>
                            item.ModuleId.ToString() == selectedItem.Value &&
                            item.RoleId == obj.ModuleMapping.RoleId
                        );
                        if (matchingItem != null)
                        {
                            if (selectedItem.Selected && matchingItem.StatusFlag == true)
                            {
                                var update = await _moduleRoleMappingService.RecoverAsync<APIResponse>(matchingItem.RoleMapId, HttpContext.Session.GetString(SD.SeesionToken));
                            }
                            else if (selectedItem.Selected == false && matchingItem.StatusFlag == false)
                            {
                                var delete = await _moduleRoleMappingService.DeleteAsync<APIResponse>(matchingItem.RoleMapId, HttpContext.Session.GetString(SD.SeesionToken));
                            }
                        }
                        else
                        {
                            if (selectedItem.Selected)
                            {
                                obj.ModuleMapping.ModuleId = int.Parse(selectedItem.Value);
                                APIResponse create = await _moduleRoleMappingService.CreateAsync<APIResponse>(obj.ModuleMapping, HttpContext.Session.GetString(SD.SeesionToken));
                            }
                        }
                    }

                    TempData["success"] = "Updated successfully";
                    return RedirectToAction(nameof(CreateModuleMapping));

                }
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            TempData["error"] = "Error encountered";
            return View(obj);
        }


      
        [Authorize(Roles = "Admin,Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateModuleMapping(ModuleMappinVM model)
        {
            if (ModelState.IsValid)
            {
                APIResponse result = await _moduleRoleMappingService.UpdateAsync<APIResponse>(model.ModuleMapping, HttpContext.Session.GetString(SD.SeesionToken));
                if (result != null && result.IsSuccess)
                {
                    TempData["success"] = "Updated successfully";
                    return RedirectToAction(nameof(IndexModuleMapping));
                }

            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
     
      
        public async Task<IActionResult> DeleteModuleMapping(int moduleId)
        {

            var response = await _moduleRoleMappingService.DeleteAsync<APIResponse>(moduleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "User deleted successfully";
                return RedirectToAction(nameof(IndexModuleMapping));
            }
            TempData["error"] = "Error encountered.";
            return View(moduleId);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnableModuleMapping(int moduleId)
        {
            if (ModelState.IsValid)
            {
                var response = await _moduleRoleMappingService.RecoverAsync<APIResponse>(moduleId, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Enabled successfully";
                    return RedirectToAction(nameof(IndexModuleMapping));
                }

                TempData["error"] = "error";
                return RedirectToAction(nameof(IndexModuleMapping));
            }

            return View();
        }

    }

        

    }





