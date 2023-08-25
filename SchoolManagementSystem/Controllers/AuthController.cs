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


    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        protected APIResponse _response;
        private readonly int _loginUserid;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor,IMapper mapper, ICategoryRepository categoryRepository)
        {
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _response = new();
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/UserApiController/GetAllRegister")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllRegister()
        {


            try
            {
                

                List<Register> RegisterListDTO = await _userRepository.GetAllRegisterAsync(includeProperties:"Categories");
              
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

        [Route("api/UserApiController/Login")]
        [HttpPost]

        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
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
        [Route("api/UserApiController/Register")]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Register")]

        public async Task<IActionResult> Register([FromBody] RegistrationDTO model)
        {

            if (!_userRepository.IsUniqueUser(model.Email))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("User Email already exists"); 
                return BadRequest(_response);

            }
            var user = await _userRepository.Register(model, _loginUserid);


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
        [HttpGet]
        [Route("api/UserApiController/GetAllUserLogin")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllUserLogin()
        {


            try
            {
                List<User> UserListDTO = await _userRepository.GetAllUserAsync();
                if (UserListDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("User Details Showed");
                _response.Result = _mapper.Map<List<UserDTO>>(UserListDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [Route("api/UserApiController/UserRegister")]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UserRegister([FromBody] UserDTO model)
        {

            if (!_userRepository.IsUniqueUser(model.UserName))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Username already exists");
                return BadRequest(_response);

            }
           
            await _userRepository.UserRegister(model, _loginUserid);


            if (model == null)
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

    }
}
