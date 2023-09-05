using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Repository;
using SchoolManagementSystem.Repository.IRepository;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    public class UserAPIController : ControllerBase
    {


        private readonly IUserRepository _userRepository;
        protected APIResponse _response;
        private readonly int _loginUserid;
        private readonly IMapper _mapper;
        public UserAPIController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userRepository = userRepository;
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _response = new();
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Register")]
        [Route("api/UserApiController/GetAllUserLogin")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllUserLogin([FromQuery] string? search)
        {


            try
            {
                IEnumerable<User> UserListDTO = await _userRepository.GetAllUserAsync(u=>u.StatusFlag==false,includeProperties: "RoleDetails,Register");

                if (!string.IsNullOrEmpty(search))
                {
                    UserListDTO = UserListDTO.Where(u => (u.UserName.ToLower().Contains(search)&&u.StatusFlag==false));
                }
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
        [HttpGet]
        [Route("api/UserApiController/GetUserId")]
        [Authorize(Roles = "Register")]

        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetUsers([FromBody] int UserId)
        {
            if (UserId == 0)
            {

                _response.Messages.Add("User ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                User userdetails = await _userRepository.GetAsync(u => (u.UserId == UserId && u.StatusFlag == false));


                if (userdetails == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<User>(userdetails);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [Route("api/UserApiController/UserRegister")]
        [HttpPost]
        [Authorize(Roles = "Register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UserRegister([FromBody] UserDTO model)
        {

            if (!_userRepository.IsUniqueUser(model.UserName, model.UserId))
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
        [HttpDelete]
        [Authorize(Roles = "Register")]

        [Route("api/UserApiController/RemoveUser")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete([FromBody] int UserId)
        {
            if (UserId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (UserId == 0)
                {
                    return BadRequest();
                }


                var Role = await _userRepository.GetAsync(u => u.UserId == UserId&&u.StatusFlag==false);
                if (Role == null)
                {
                    return NotFound();
                }

                Role.StatusFlag = true;
                await _userRepository.UpdateAsync(Role, _loginUserid);
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

        [Route("api/UserApiController/UpdateUser")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] User user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.IsUniqueUser(user.UserName,user.UserId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Username already exists");
                return BadRequest(_response);

            }
            try
            {
                if (user == null)
                {
                    return BadRequest();

                }

                User model = _mapper.Map<User>(user);

                await _userRepository.UpdateAsync(model, _loginUserid);
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


