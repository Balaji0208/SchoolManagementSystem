using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Repository;
using SchoolManagementSystem.Repository.IRepository;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    public class StateMasterAPIController:ControllerBase
    {
        private readonly IStateMasterRepository _stateRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly int _loginUserid;


        public StateMasterAPIController(IStateMasterRepository stateRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
            _response = new();
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/StateMasterAPIController/GetAllCategary")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllCategary()
        {


            try
            {
                IEnumerable<StateMaster> StateListDTO = await _stateRepository.GetAllAsync(includeProperties: "CountryMaster");
                if (StateListDTO == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<List<StateMasterDTO>>(StateListDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet]
        [Route("api/StateMasterAPIController/GetCategary")]
        [Authorize(Roles = "Admin")]
       
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetCategary([FromBody]int stateId)
        {
            if (stateId == 0)
            {

                _response.Messages.Add("Role ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                StateMaster StateDetails= await _stateRepository.GetAsync(u => (u.StateId == stateId&& u.StatusFlag == false));


                if (StateDetails == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<StateMaster>(StateDetails);
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
        [Route("api/StateMasterAPIController/Create")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] StateMasterDTO stateDTO)
        {

            try
            {
             

                if (!_stateRepository.IsUniqueName(stateDTO.StateName,stateDTO.StateId))
                {
                    ModelState.AddModelError("ErrorMessages", "Category Name Already Exists");
                    return BadRequest(ModelState);
                }
                if (stateDTO == null)
                {
                    return BadRequest(stateDTO);

                }
              


            


                await _stateRepository.CreateAsync(stateDTO, _loginUserid);

                _response.Result = _mapper.Map<StateMaster>(stateDTO);
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
        [HttpGet]
        [Route("api/StateMasterAPIController/GetAllStateByCountryId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllStateByCountryId([FromBody]int countryId)
        {
            try { 
                if (countryId == 0) { 
                    _response.StatusCode = HttpStatusCode.BadRequest; 
                    return BadRequest(_response); 
                } 
                IEnumerable<StateMaster> stateList = await _stateRepository.GetAllAsync(u => u.CountryId == countryId && u.StatusFlag == false); 
                if (stateList == null)
                { 
                    _response.StatusCode = HttpStatusCode.NotFound; 
                    return NotFound(_response); 
                } 
                _response.Result = _mapper.Map<List<StateMasterDTO>>(stateList); 
                _response.StatusCode = HttpStatusCode.OK; return Ok(_response); 
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
      
        [Route("api/StateMasterAPIController/Delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete([FromBody]int stateId)
        {
            if (stateId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (stateId == 0)
                {
                    return BadRequest();
                }


                var state = await _stateRepository.GetAsync(u => u.StateId == stateId);
                if (state == null)
                {
                    return NotFound();
                }

                state.StatusFlag = true;
                await _stateRepository.UpdateAsync(state, _loginUserid);
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
       
        [Route("api/StateMasterAPIController/Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] StateMaster state)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_stateRepository.IsUniqueName(state.StateName, state.StateId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("User Email already exists");
                return BadRequest(_response);

            }
            try
            {
                if (state == null)
                {
                    return BadRequest();

                }

                StateMaster model = _mapper.Map<StateMaster>(state);

                await _stateRepository.UpdateAsync(model, _loginUserid);
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

    [Route("api/StateMasterAPI/EnableState")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<ActionResult<APIResponse>> EnableSate([FromBody] int stateId)
    {
        try
        {
            if (stateId == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var stateDTO = await _stateRepository.GetAsync(u => u.StateId == stateId && u.StatusFlag == true);

            if (stateDTO == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            StateMaster state = _mapper.Map<StateMaster>(stateDTO);



                state.StatusFlag = false;
            await _stateRepository.UpdateAsync(state, _loginUserid);

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
