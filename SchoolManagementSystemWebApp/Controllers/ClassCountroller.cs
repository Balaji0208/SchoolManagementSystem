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
    public class ClassController : Controller
    {
       
            private readonly IClassService _classService;
           

           
            public ClassController(IClassService classService)
            {
                _classService = classService;
               
            }

            public async Task<IActionResult> IndexClass()
            {
                List<ClassMasterDTO> list = new();
                var response = await _classService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ClassMasterDTO>>(Convert.ToString(response.Result));
                }
                return View(list);
            }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateClass()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClass(ClassMasterDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _classService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(IndexClass));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateClass(int roleId)
        {
            var response = await _classService.GetAsync<APIResponse>(roleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {

                ClassMasterDTO model = JsonConvert.DeserializeObject<ClassMasterDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClass(ClassMasterDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Villa updated successfully";
                var response = await _classService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexClass));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteClass(int roleId)
        {
            var response = await _classService.GetAsync<APIResponse>(roleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                ClassMasterDTO model = JsonConvert.DeserializeObject<ClassMasterDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClass(ClassMasterDTO model)
        {

            var response = await _classService.DeleteAsync<APIResponse>(model.ClassId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(IndexClass));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }


    }
}
       
