using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Repository.IRepository;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Models;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    public class RoleMasterAPIController : ControllerBase
    {
        private readonly IRoleMasterRepository _rolemasterRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly int _loginUserid;


        public RoleMasterAPIController(IRoleMasterRepository rolemasterRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _rolemasterRepository = rolemasterRepository;
            _mapper = mapper;
            _response = new();
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        }

        [HttpGet]
        [Route("api/RoleMasterAPI/GetRols")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetRols()
        {


            try
            {
                List<RoleDetails> roleListDTO = await _rolemasterRepository.GetAllAsync(u=>u.StatusFlag == false);
                if (roleListDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<List<RoleDetailsDTO>>(roleListDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet]
        [Route("api/RoleMasterAPI/GetRole")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetRole(int RoleId)
        {
            if (RoleId == 0)
            {

                _response.Messages.Add("Role ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                RoleDetails roledetails = await _rolemasterRepository.GetAsync(u => (u.RoleId == RoleId && u.StatusFlag == false));


                if (roledetails == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<RoleDetails>(roledetails);
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
       [Authorize(Roles = "Register")]
        [Route("api/RoleMasterAPI/Create")]

        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] RoleDetailsDTO rolemasterDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }

                if (await _rolemasterRepository.GetAsync(u => u.RoleName.ToLower() == rolemasterDTO.RoleName.ToLower()) != null)

                {
                    ModelState.AddModelError("ErrorMessages", "Role Already Exists");
                    return BadRequest(ModelState);
                }
                if (rolemasterDTO == null)
                {
                    return BadRequest(rolemasterDTO);


                }

                RoleDetails Role = _mapper.Map<RoleDetails>(rolemasterDTO);



                await _rolemasterRepository.CreateAsync(Role,_loginUserid);

                _response.Result = _mapper.Map<RoleDetailsDTO>(Role);
                _response.StatusCode = HttpStatusCode.Created;

                return Ok(rolemasterDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages= new List<string>() { ex.ToString() };
            }
            return _response;
        }

    


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("api/RoleMasterAPI/RemoverRole")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete(int RoleId)
        {
            if (RoleId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (RoleId == 0)
                {
                    return BadRequest();
                }
                

                var Role = await _rolemasterRepository.GetAsync(u => u.RoleId == RoleId);
                if (Role == null)
                {
                    return NotFound();
                }
                
                Role.StatusFlag = true;
                await _rolemasterRepository.UpdateAsync(Role,_loginUserid);
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
        [Route("api/RoleMasterAPI/UpdateRoleMaster")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] RoleDetails rolemaster)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (rolemaster == null )
                {
                    return BadRequest();

                }
              
                RoleDetails model = _mapper.Map<RoleDetails>(rolemaster);
               
                await _rolemasterRepository.UpdateAsync(model, _loginUserid);
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
