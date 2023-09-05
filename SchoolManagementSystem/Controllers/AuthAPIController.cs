using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AutoMapper;
using SchoolManagementSystem.Repository;

namespace SchoolManagementSystem.Controllers
{


    public class AuthApiController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICategoryRepository _categoryRepository;
        protected APIResponse _response;
        private readonly int _loginUserid;
        private readonly IMapper _mapper;
        public AuthApiController(IAuthRepository authRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _authRepository = authRepository;
            _categoryRepository = categoryRepository;
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _response = new();
            _mapper = mapper;
        }
        [Authorize(Roles = "Register")]
        [HttpGet]
        [Route("api/AuthApiController/GetAllRegister")]
        [ProducesResponseType(200)]

        public async Task<ActionResult<APIResponse>> GetAllRegister()
        {


            try
            {


                List<Register> RegisterListDTO = await _authRepository.GetAllRegisterAsync ( u => u.StatusFlag == false,includeProperties: "Categories");

                if (RegisterListDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<List<RegistrationDTO>>(RegisterListDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }
        [HttpGet]
        [Route("api/AuthApiController/GetRegisterId")]
        [Authorize(Roles = "Register")]

        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetRegisterId([FromBody]int regId)
        {
            if (regId == 0)
            {

                _response.Messages.Add("Register ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                Register register = await _authRepository.GetAsync(u => (u.registerId == regId && u.StatusFlag == false));


                if (register == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Register Details Showed");
                _response.Result = _mapper.Map<Register>(register);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [Route("api/AuthApiController/Login")]
        [HttpPost]

        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _authRepository.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Username  or password is incorrect");
                return BadRequest(_response);

            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);

        }
        [Authorize(Roles = "Register")]
        [Route("api/AuthApiController/Register")]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        //Register

        public async Task<IActionResult> Register([FromBody] RegistrationDTO model)
        {

            if (!_authRepository.IsUniqueUser(model.Email,model.registerId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("User Email already exists");
                return BadRequest(_response);

            }
            var user = await _authRepository.Register(model, _loginUserid);


            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Error while registering");
                return BadRequest(_response);



            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        [HttpDelete]
        [Authorize(Roles = "Register")]

        [Route("api/AuthApiController/Remove")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete([FromBody]int RegId)
        {
            if (RegId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (RegId == 0)
                {
                    return BadRequest();
                }


                var Role = await _authRepository.GetAsync(u => u.registerId == RegId);
                if (Role == null)
                {
                    return NotFound();
                }

                Role.StatusFlag = true;
                await _authRepository.UpdateAsync(Role, _loginUserid);
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

        [Route("api/AuthApiController/Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] RegistrationDTO register)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (register == null)
                {
                    return BadRequest();

                }

                Register model = _mapper.Map<Register>(register);

                await _authRepository.UpdateAsync(model, _loginUserid);
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
        [HttpGet]
        [Route("api/AuthApiController/GetUserByPrefix")]
        public async Task<IActionResult> Search([FromBody] string Prefix)
        {
            var matchingUser = await _authRepository.GetUserByPrefix(Prefix);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Messages.Add("Role Details Showed");
            _response.Result = (matchingUser);
            return Ok(_response);
        }
        [HttpGet]
        [Route("api/AuthApiController/GetAllDeletedRegistration")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetAllDeletedRegistration()
        {
            try
            {
                IEnumerable<Register> registrationList = await _authRepository.GetAllRegisterAsync(u => (u.StatusFlag), includeProperties: "CategoryMaster,StateMaster,CountryMaster");
                _response.Result = _mapper.Map<List<RegistrationDTO>>(registrationList);
                _response.StatusCode = HttpStatusCode.OK;
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
        [Route("api/AuthApiController/EnableRegistration")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> EnableRegistration(int registrationId)
        {
            try
            {
                if (registrationId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var registrationDTO = await _authRepository.GetAsync(u => u.registerId == registrationId && u.StatusFlag == true);

                if (registrationDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                Register registrationDetail = _mapper.Map<Register>(registrationDTO);

                

                registrationDetail.StatusFlag = false;
                await _authRepository.UpdateAsync(registrationDetail,_loginUserid);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages= new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}




  
