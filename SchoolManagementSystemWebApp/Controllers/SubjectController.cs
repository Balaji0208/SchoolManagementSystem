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
    public class SubjectController : Controller
    {
       
            private readonly ISubjectService _subjectService;
           

           
            public SubjectController(ISubjectService subjectService)
            {
                _subjectService = subjectService;
               
            }

            public async Task<IActionResult> IndexSubject()
            {
                List<SubjectMasterDTO> list = new();
                var response = await _subjectService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<SubjectMasterDTO>>(Convert.ToString(response.Result));
                }
                return View(list);
            }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateSubject()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(SubjectMasterDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _subjectService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(IndexSubject));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateSubject(int roleId)
        {
            var response = await _subjectService.GetAsync<APIResponse>(roleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {

                SubjectMasterDTO model = JsonConvert.DeserializeObject<SubjectMasterDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSubject(SubjectMasterDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Villa updated successfully";
                var response = await _subjectService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexSubject));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteSubject(int roleId)
        {
            var response = await _subjectService.GetAsync<APIResponse>(roleId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                SubjectMasterDTO model = JsonConvert.DeserializeObject<SubjectMasterDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubject(SubjectMasterDTO model)
        {

            var response = await _subjectService.DeleteAsync<APIResponse>(model.SubjectId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(IndexSubject));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }


    }
}
       