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
    public class StateController : Controller
    {
       
            private readonly IStateService _stateService;
        private readonly ICountryService _countryService;


        public StateController(IStateService stateService, ICountryService countryService)
        {
            _stateService = stateService;
            _countryService = countryService;
        }

        public async Task<IActionResult> IndexState(int currentPage = 1, string orederBy = "", string term = "")
            {
            StatePaginationVM stateVM = new StatePaginationVM();

            IEnumerable<StateMasterDTO> list = null;

            var response = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<StateMasterDTO>>(Convert.ToString(response.Result));
            }

            int totalRecords = list.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            stateVM.State = list;
            stateVM.CurrentPage = currentPage;
            stateVM.PageSize = pageSize;
            stateVM.TotalPages = totalPages;
            stateVM.OrderBy = orederBy;
            return View(stateVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateState()
        {
            StateVM stateVM = new();
            var Country = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (Country != null && Country.IsSuccess)
            {
                stateVM.StateList = JsonConvert.DeserializeObject<List<CountryMasterDTO>>
                  (Convert.ToString(Country.Result)).Select(i => new SelectListItem
                  {
                      Text = i.CountryName,
                      Value = i.CountryId.ToString()
                  });
            }

            return View(stateVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateState(StateVM model)
        {
            if (ModelState.IsValid)
            {

                APIResponse response = await _stateService.CreateAsync<APIResponse>(model.StateRegistration, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(IndexState));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateState(int stateId)
        {
            StateVM stateVM = new();
            var response = await _stateService.GetAsync<APIResponse>(stateId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {

                StateMasterDTO model = JsonConvert.DeserializeObject<StateMasterDTO>(Convert.ToString(response.Result));
                stateVM.StateRegistration = model;
            }
            var Country = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));

            if (Country != null && Country.IsSuccess)
            {
                 stateVM.StateList= JsonConvert.DeserializeObject<List<CountryMasterDTO>>
                  (Convert.ToString(Country.Result)).Select(i => new SelectListItem
                  {
                      Text = i.CountryName,
                      Value = i.CountryId.ToString()
                  });
            }
            return View(stateVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateState(StateVM model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "State updated successfully";
                APIResponse response = await _stateService.UpdateAsync<APIResponse>(model.StateRegistration, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexState));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
     
        public async Task<IActionResult> DeleteState(StateMasterDTO model)
        {

            var response = await _stateService.DeleteAsync<APIResponse>(model.StateId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "State deleted successfully";
                return RedirectToAction(nameof(IndexState));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        public async Task<IActionResult> EnableState(int stateId)
        {
            if (ModelState.IsValid)
            {
                var response = await _stateService.RecoverAsync<APIResponse>(stateId, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Enabled successfully";
                    return RedirectToAction(nameof(IndexState));
                }

                TempData["error"] = "error";
                return RedirectToAction(nameof(IndexState));
            }

            return View();
        }



    }
}
       
