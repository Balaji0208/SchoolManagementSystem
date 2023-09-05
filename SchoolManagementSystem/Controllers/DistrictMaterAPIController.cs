using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Repository.IRepository;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    public class DistrictMasterAPIController:ControllerBase
    {
        private readonly IDistrictMasterRepository _districtRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly int _loginUserid;


        public DistrictMasterAPIController(IDistrictMasterRepository districtRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;
            _response = new();
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        }

        [HttpGet]
        [Authorize(Roles = "Register")]
        [Route("api/DistrictMasterAPI/GetAllCategary")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllDistrict()
        {


            try
            {
                List<DistrictMaster> DistrictListDTO = await _districtRepository.GetAllAsync(u=> u.StatusFlag == false);
                if (DistrictListDTO == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<List<DistrictMasterDTO>>(DistrictListDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet]
        [Route("api/DistrictMasterAPI/GetCategary")]
        [Authorize(Roles = "Register")]
       
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetDistriict(int districtId)
        {
            if (districtId == 0)
            {

                _response.Messages.Add("Role ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                DistrictMaster DistrictDetails= await _districtRepository.GetAsync(u => (u.DistrictId == districtId&& u.StatusFlag == false));


                if (DistrictDetails == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<Categories>(DistrictDetails);
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
        [Route("api/DistrictMasterAPI/Create")]
        [Authorize(Roles = "Register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] DistrictMasterDTO DistrictDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }

                if (await _districtRepository.GetAsync(u => u.DistrictName.ToLower() == DistrictDTO.DistrictName.ToLower()) != null)

                {
                    ModelState.AddModelError("ErrorMessages", "Category Name Already Exists");
                    return BadRequest(ModelState);
                }
                if (DistrictDTO == null)
                {
                    return BadRequest(DistrictDTO);


                }

                DistrictMaster District = _mapper.Map<DistrictMaster>(DistrictDTO);



                await _districtRepository.CreateAsync(District, _loginUserid);

                _response.Result = _mapper.Map<CategoryDTO>(District);
                _response.StatusCode = HttpStatusCode.Created;

                return Ok(_response);
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
      
        [Route("api/DistrictMasterAPI/Delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete(int districtId)
        {
            if (districtId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (districtId == 0)
                {
                    return BadRequest();
                }


                var district = await _districtRepository.GetAsync(u => u.DistrictId == districtId);
                if (districtId == null)
                {
                    return NotFound();
                }

                district.StatusFlag = true;
                await _districtRepository.UpdateAsync(district, _loginUserid);
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
       
        [Route("api/DistrictMasterAPI/Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] DistrictMaster district)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (district == null)
                {
                    return BadRequest();

                }

                DistrictMaster model = _mapper.Map<DistrictMaster>(district);

                await _districtRepository.UpdateAsync(model, _loginUserid);
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
