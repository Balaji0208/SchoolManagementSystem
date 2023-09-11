using AutoMapper;
using Azure;
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
    public class CountryMasterAPIController : ControllerBase
    {
        private readonly ICountryMasterRepository _countrymasterRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly int _loginUserid;


        public CountryMasterAPIController(ICountryMasterRepository countrymasterRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _countrymasterRepository = countrymasterRepository;
            _mapper = mapper;
            _response = new();
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/CountryMasterAPI/GetAllCountry")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllCountry()
        {


            try
            {
                List<CountryMaster> CountryMasterDTO = await _countrymasterRepository.GetAllAsync();
                if (CountryMasterDTO == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<List<CountryMaster>>(CountryMasterDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet]
        [Route("api/CategoryMasterAPI/GetCountry")]
        [Authorize(Roles = "Admin")]

        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetCountry([FromBody] int countryId)
        {
            if (countryId == 0)
            {

                _response.Messages.Add("Role ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                CountryMaster countryMaster = await _countrymasterRepository.GetAsync(u => (u.CountryId == countryId && u.StatusFlag == false));


                if (countryMaster == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<CountryMaster>(countryMaster);
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
        [Route("api/CountryMasterAPI/Create")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] CountryMasterDTO countryDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }

                if (await _countrymasterRepository.GetAsync(u => u.CountryName.ToLower() == countryDTO.CountryName.ToLower()) != null)

                {
                    ModelState.AddModelError("ErrorMessages", "Category Name Already Exists");
                    return BadRequest(ModelState);
                }
                if (countryDTO == null)
                {
                    return BadRequest(countryDTO);


                }
                CountryMaster Country = _mapper.Map<CountryMaster>(countryDTO);






                await _countrymasterRepository.CreateAsync(Country, _loginUserid);

                _response.Result = _mapper.Map<CountryMaster>(Country);
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
        [Authorize(Roles = "Admin")]

        [Route("api/CountryMasterAPI/Delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete([FromBody] int countryId)
        {
            if (countryId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (countryId == 0)
                {
                    return BadRequest();
                }


                var country = await _countrymasterRepository.GetAsync(u => u.CountryId == countryId);
                if (country == null)
                {
                    return NotFound();
                }

                country.StatusFlag = true;
                await _countrymasterRepository.UpdateAsync(country, _loginUserid);
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
        [Authorize(Roles = "Admin")]

        [Route("api/CountryMasterAPI/Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] CountryMaster country)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_countrymasterRepository.IsUniqueName(country.CountryName, country.CountryId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("User Email already exists");
                return BadRequest(_response);

            }
            try
            {
                if (country == null)
                {
                    return BadRequest();

                }

                CountryMaster model = _mapper.Map<CountryMaster>(country);

                await _countrymasterRepository.UpdateAsync(model, _loginUserid);
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
        [Authorize(Roles = "Admin")]

        [Route("api/CountryMasterAPI/EnableCountry")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> EnableCountry([FromBody] int countryId)
        {
            try
            {
                if (countryId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var countryDTO = await _countrymasterRepository.GetAsync(u => u.CountryId == countryId && u.StatusFlag == true);

                if (countryDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                CountryMaster country = _mapper.Map<CountryMaster>(countryDTO);



                country.StatusFlag = false;
                await _countrymasterRepository.UpdateAsync(country, _loginUserid);

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
