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
    public class CountryController : Controller
    {

        private readonly ICountryService _countryService;



        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;

        }

        public async Task<IActionResult> IndexCountry()
        {
            List<CountryMasterDTO> list = new();
            var response = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CountryMasterDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize(Roles = "Register")]
        public async Task<IActionResult> CreateCountry()
        {
            return View();
        }
        [Authorize(Roles = "Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry(CountryMasterDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _countryService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(IndexCountry));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Register")]
        public async Task<IActionResult> UpdateCountry(int countryId)
        {
            var response = await _countryService.GetAsync<APIResponse>(countryId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {

                CountryMasterDTO model = JsonConvert.DeserializeObject<CountryMasterDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCountry(CountryMasterDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Villa updated successfully";
                var response = await _countryService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexCountry));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
      
       
        public async Task<IActionResult> DeleteCountry(int countryId)
        {

            var response = await _countryService.DeleteAsync<APIResponse>(countryId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(IndexCountry));
            }
            TempData["error"] = "Error encountered.";
            return View();
        }


    }
}

