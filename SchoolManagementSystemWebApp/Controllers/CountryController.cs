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
    public class CountryController : Controller
    {

        private readonly ICountryService _countryService;



        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexCountry(int currentPage =1, string orederBy = "", string term = "")
        {
            CountryPaginationVM countryVM = new CountryPaginationVM();

            IEnumerable<CountryMasterDTO> list = new List<CountryMasterDTO>();

            var response = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CountryMasterDTO>>(Convert.ToString(response.Result));
            }

            int totalRecords = list.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            countryVM.Country = list;
            countryVM.CurrentPage = currentPage;
            countryVM.PageSize = pageSize;
            countryVM.TotalPages = totalPages;
            countryVM.OrderBy = orederBy;
            return View(countryVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCountry()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> EnableCountry(int countryId)
        {
            if (ModelState.IsValid)
            {
                var response = await _countryService.RecoverAsync<APIResponse>(countryId, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Enabled successfully";
                    return RedirectToAction(nameof(IndexCountry));
                }

                TempData["error"] = "error";
                return RedirectToAction(nameof(IndexCountry));
            }

            return View();
        }


    }
}

