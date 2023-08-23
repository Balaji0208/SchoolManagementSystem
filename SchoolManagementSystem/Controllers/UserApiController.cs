using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
 

    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response;
        private readonly int _loginUserid;
        public UsersController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _response = new();
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

         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO model)
        {
           
            if (!_userRepository.IsUniqueUser(model.UserName))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Username already exists");
                return BadRequest(_response);

            }
            var user = await _userRepository.Register(model, _loginUserid);


            if ( user== null)
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
