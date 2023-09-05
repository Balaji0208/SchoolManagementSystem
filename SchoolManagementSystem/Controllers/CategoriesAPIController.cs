using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Repository;
using SchoolManagementSystem.Repository.IRepository;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    public class CategoriesAPIController:ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly int _loginUserid;


        public CategoriesAPIController(ICategoryRepository rolemasterRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = rolemasterRepository;
            _mapper = mapper;
            _response = new();
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        }

        [HttpGet]
        [Authorize(Roles = "Register")]
        [Route("api/CategoryMasterAPI/GetAllCategary")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllCategary()
        {


            try
            {
                List<Categories> CategoryListDTO = await _categoryRepository.GetAllAsync(u=> u.StatusFlag == false);
                if (CategoryListDTO == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<List<CategoryDTO>>(CategoryListDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet]
        [Route("api/CategoryMasterAPI/GetCategary")]
        [Authorize(Roles = "Register")]
       
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetCategary([FromBody]int categoryId)
        {
            if (categoryId == 0)
            {

                _response.Messages.Add("Role ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                Categories categoryDetails= await _categoryRepository.GetAsync(u => (u.CategoryId == categoryId&& u.StatusFlag == false));


                if (categoryDetails == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<Categories>(categoryDetails);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Register")]
        [Route("api/CategaryMasterAPI/Create")]
        [Authorize(Roles = "Register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] CategoryDTO categoryDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }

                if (await _categoryRepository.GetAsync(u => u.CategoryName.ToLower() == categoryDTO.CategoryName.ToLower()) != null)

                {
                    ModelState.AddModelError("ErrorMessages", "Category Name Already Exists");
                    return BadRequest(ModelState);
                }
                if (categoryDTO == null)
                {
                    return BadRequest(categoryDTO);


                }

                Categories category= _mapper.Map<Categories>(categoryDTO);



                await _categoryRepository.CreateAsync(category, _loginUserid);

                _response.Result = _mapper.Map<CategoryDTO>(category);
                _response.StatusCode = HttpStatusCode.Created;

                return Ok(categoryDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }
            return _response;
        }




        [HttpDelete]
        [Authorize(Roles = "Register")]
      
        [Route("api/CategaryMasterAPI/Delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete([FromBody]int categoryId)
        {
            if (categoryId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (categoryId == 0)
                {
                    return BadRequest();
                }


                var category = await _categoryRepository.GetAsync(u => u.CategoryId == categoryId);
                if (category == null)
                {
                    return NotFound();
                }

                category.StatusFlag = true;
                await _categoryRepository.UpdateAsync(category, _loginUserid);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }
            return _response;
        }



        [HttpPut]
        [Authorize(Roles = "Register")]
       
        [Route("api/CategaryMasterAPI/Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] Categories category)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_categoryRepository.IsUniqueName(category.CategoryName, category.CategoryId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("User Email already exists");
                return BadRequest(_response);

            }
            try
            {
                if (category == null)
                {
                    return BadRequest();

                }

                Categories model = _mapper.Map<Categories>(category);

                await _categoryRepository.UpdateAsync(model, _loginUserid);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}
