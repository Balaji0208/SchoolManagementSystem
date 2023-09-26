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
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;



        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexCategory(int currentPage = 1, string orederBy = "", string term = "")
        {
            CategoryPaginationVM categoryPagination = new CategoryPaginationVM();

            IEnumerable<CategoriesDTO> list = new List<CategoriesDTO>();

            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoriesDTO>>(Convert.ToString(response.Result));
            }

            int totalRecords = list.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            categoryPagination.Category = list;
            categoryPagination.CurrentPage = currentPage;
            categoryPagination.PageSize = pageSize;
            categoryPagination.TotalPages = totalPages;
            categoryPagination.OrderBy = orederBy;
            return View(categoryPagination);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoriesDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _categoryService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(IndexCategory));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int categoryId)
        {
            var response = await _categoryService.GetAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {

                CategoriesDTO model = JsonConvert.DeserializeObject<CategoriesDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(CategoriesDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Villa updated successfully";
                var response = await _categoryService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SeesionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexCategory));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
      
     
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {

            var response = await _categoryService.DeleteAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SeesionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction(nameof(IndexCategory));
            }
            TempData["error"] = "Error encountered.";
            return View();
        }
        public async Task<IActionResult> EnableCategory(int categoryId)
        {
            if (ModelState.IsValid)
            {
                var response = await _categoryService.RecoverAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SeesionToken));

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Enabled successfully";
                    return RedirectToAction(nameof(IndexCategory));
                }

                TempData["error"] = "error";
                return RedirectToAction(nameof(IndexCategory));
            }

            return View();
        }



    }
}

