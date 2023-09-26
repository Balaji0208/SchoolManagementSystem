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
    public class ModuleController : Controller
    {
       
            private readonly IModuleService _moduleService;
       


        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexModule(int currentPage = 1, string orederBy = "", string term = "")
            {
            ModulePaginationVM moduleVM = new ModulePaginationVM();

            IEnumerable<ModuleDTO> list = new List<ModuleDTO>();

            var response = await _moduleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ModuleDTO>>(Convert.ToString(response.Result));
            }

            int totalRecords = list.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            moduleVM.Module = list;
            moduleVM.CurrentPage = currentPage;
            moduleVM.PageSize = pageSize;
            moduleVM.TotalPages = totalPages;
            moduleVM.OrderBy = orederBy;
            return View(moduleVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateModule()
        {
            
              ModulesVM modulesVM = new();
            var ParenMenu = await _moduleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (ParenMenu != null && ParenMenu.IsSuccess)
            {
                modulesVM.ParentList = JsonConvert.DeserializeObject<List<ModuleDTO>>
                  (Convert.ToString(ParenMenu.Result)).Select(i => new SelectListItem
                  {
                      Text = i.Menus,
                      Value = i.ParentId.ToString()
                  });
            }

            return View(modulesVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModule(ModulesVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.modulesVM.ParentId == null)
                {
                    model.modulesVM.ParentId = 0;
                }
                APIResponse response = await _moduleService.CreateAsync<APIResponse>(model.modulesVM, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(IndexModule));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateModule(int moduleId)
        {
            ModulesVM moduleVM = new();
            var response = await _moduleService.GetAsync<APIResponse>(moduleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {

                ModuleDTO model = JsonConvert.DeserializeObject<ModuleDTO>(Convert.ToString(response.Result));
                moduleVM.modulesVM = model;
            }
            var parentMenu = await _moduleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (parentMenu != null && parentMenu.IsSuccess)
            {
                moduleVM.ParentList= JsonConvert.DeserializeObject<List<ModuleDTO>>
                  (Convert.ToString(parentMenu.Result)).Select(i => new SelectListItem
                  {
                      Text = i.Menus,
                      Value = i.ParentId.ToString()
                  });
            }
            return View(moduleVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateModule(ModulesVM model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "State updated successfully";
                APIResponse response = await _moduleService.UpdateAsync<APIResponse>(model.modulesVM, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexModule));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
     
        public async Task<IActionResult> DeleteModule(ModuleDTO model)
        {

            var response = await _moduleService.DeleteAsync<APIResponse>(model.ModuleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "State deleted successfully";
                return RedirectToAction(nameof(IndexModule));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        public async Task<IActionResult> EnableModule(int moduleId)
        {
            if (ModelState.IsValid)
            {
                var response = await _moduleService.RecoverAsync<APIResponse>(moduleId, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Enabled successfully";
                    return RedirectToAction(nameof(IndexModule));
                }

                TempData["error"] = "error";
                return RedirectToAction(nameof(IndexModule));
            }

            return View();
        }



    }
}
       
